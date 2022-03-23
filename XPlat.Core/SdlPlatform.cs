using System;
using System.Drawing;
using System.Numerics;
using GLES2;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SDL2;

namespace XPlat.Core
{
    public class SdlPlatform : ISdlPlatformEvents, IPlatform
    {
        private SDL.SDL_Event @event;
        private IntPtr window;
        private IntPtr context;
        private bool isRunning = false;
        private Vector2 windowSize;

        private readonly ILogger<SdlPlatform> logger;
        private readonly IHostApplicationLifetime lifetime;
        private readonly PlatformEventHandler<SDL.SDL_EventType, SDL.SDL_Event>[] handlers = new PlatformEventHandler<SDL.SDL_EventType, SDL.SDL_Event>[(int)SDL.SDL_EventType.SDL_LASTEVENT];

        public Vector2 RendererSize { get; private set; }
        public bool AutoSwap { get; set; } = true;
        public int TargetFramerate { get; set; } = 30;

        public Vector2 WindowSize
        {
            get => windowSize; 
            set
            {
                SDL.SDL_SetWindowSize(window, (int)value.X, (int)value.Y);
                UpdateWindow();
            }
        }

        public Vector2 MousePosition { get; private set; }

        public float RetinaScale { get; private set; }
        public string WindowTitle { get => SDL.SDL_GetWindowTitle(window); set => SDL.SDL_SetWindowTitle(window, value); }

        public event PlatformEventHandler<SDL.SDL_EventType, SDL.SDL_Event> OnEvent;

        public SdlPlatform(ILogger<SdlPlatform> logger,
                           IHostApplicationLifetime lifetime)
        {
            this.logger = logger;
            this.lifetime = lifetime;

            lifetime.ApplicationStopping.Register(() => isRunning = false);
        }

        public void Init()
        {
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
            logger.LogInformation("SDL initialized");

            SDL.SDL_SetHint("SDL_OPENGL_ES_DRIVER", "1");
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 2);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 0);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_ES);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_RED_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_GREEN_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_BLUE_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_STENCIL_SIZE, 8);

            window = SDL.SDL_CreateWindow("SDL running on .NET 6.0", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 1280, 720,
            SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
            logger.LogInformation("Window initialized");

            context = SDL.SDL_GL_CreateContext(window);
            SDL.SDL_GL_SetSwapInterval(0);
            logger.LogInformation("OpengGL Context created");

            UpdateWindow();
            UpdateMouse();
            isRunning = true;
        }

        private void UpdateMouse()
        {
            int w, h;
            SDL.SDL_GetMouseState(out w, out h);
            MousePosition = new Vector2(w, h);
        }

        private void UpdateWindow()
        {
            int w, h;
            SDL.SDL_GL_GetDrawableSize(window, out w, out h);
            RendererSize = new Vector2(w, h);

            SDL.SDL_GetWindowSize(window, out w, out h);
            windowSize = new Vector2(w, h);

            RetinaScale = RendererSize.X / WindowSize.X;
        }

        public void Run(ISdlApp app)
        {
            app.Init();
            logger.LogInformation("App initialized");
            double last = 0;
            while (isRunning)
            {
                UnmanagedQueue.Purge();

                while (SDL.SDL_PollEvent(out @event) == 1)
                {

                    switch (@event.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            logger.LogInformation("Received SDL_QUIT");
                            return;
                        case SDL.SDL_EventType.SDL_WINDOWEVENT:                            
                            UpdateWindow();
                            break;
                    }
                    OnEvent?.Invoke(@event.type, ref @event);
                    handlers[(int)@event.type]?.Invoke(@event.type, ref @event);
                }
                UpdateMouse();

                GL.Viewport(0, 0, (uint)RendererSize.X, (uint)RendererSize.Y);

                app.Update();

                //logger.LogInformation("wait: " + left);

                if (AutoSwap)
                    SwapBuffers();
                
                var now = (SDL.SDL_GetPerformanceCounter() / (double)SDL.SDL_GetPerformanceFrequency()) * 1000;
                var elapsed = now - last;
                var left = 1000 / (double)TargetFramerate - elapsed;
                
                if(left > 0)
                    SDL.SDL_Delay((uint)left);

                last = (SDL.SDL_GetPerformanceCounter() / (double)SDL.SDL_GetPerformanceFrequency()) * 1000; 
            }
        }

        public void SwapBuffers() => SDL.SDL_GL_SwapWindow(window);

        public void Subscribe(SDL.SDL_EventType ev, PlatformEventHandler<SDL.SDL_EventType, SDL.SDL_Event> handler)
            => handlers[(int)ev] += handler;

        public void Unsubscribe(SDL.SDL_EventType ev, PlatformEventHandler<SDL.SDL_EventType, SDL.SDL_Event> handler)
            => handlers[(int)ev] -= handler;
    }
}