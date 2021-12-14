using System.Drawing;
using System.Numerics;
using GLES2;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SDL2;

namespace net6test
{
    public class SdlPlatform : ISdlPlatformEvents
    {
        private SDL.SDL_Event @event;
        private IntPtr window;
        private IntPtr context;
        private bool isRunning = false;
        private readonly ILogger<SdlPlatform> logger;
        private readonly IHostApplicationLifetime lifetime;
        private readonly PlatformInfo platformInfo;
        private readonly PlatformEventHandler<SDL.SDL_EventType,SDL.SDL_Event>[] handlers = new PlatformEventHandler<SDL.SDL_EventType,SDL.SDL_Event>[(int)SDL.SDL_EventType.SDL_LASTEVENT];

        public event PlatformEventHandler<SDL.SDL_EventType, SDL.SDL_Event> OnEvent;

        public SdlPlatform(ILogger<SdlPlatform> logger,
                           IHostApplicationLifetime lifetime,
                           PlatformInfo platformInfo)
        {
            this.logger = logger;
            this.lifetime = lifetime;
            this.platformInfo = platformInfo;

            lifetime.ApplicationStopping.Register(() => isRunning = false);
        }

        public void Init(ISdlApp app)
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

            app.Init();
            logger.LogInformation("App initialized");
            isRunning = true;
        }

        private void UpdateMouse()
        {
            int w, h;
            SDL.SDL_GetMouseState(out w, out h);
            platformInfo.MousePosition = new Point((int)(w * platformInfo.RetinaScale),(int)(h * platformInfo.RetinaScale));

        }

        private void UpdateWindow(){
            int w, h;
            SDL.SDL_GL_GetDrawableSize(window, out w, out h);
            platformInfo.RendererSize = new Size(w, h);
            GL.Viewport(0, 0, (uint)w, (uint)h);
            SDL.SDL_GetWindowSize(window, out w, out h);
            platformInfo.WindowSize = new Size(w, h);
        }

        public void Run(ISdlApp app)
        {
            // int w,h;
            // SDL.SDL_GL_GetDrawableSize(window, out w, out h);
            // GL.glViewport(0,0,(uint)w,(uint)h);

            // app.Update();
            // SDL.SDL_GL_SwapWindow(window);

            while (isRunning)
            {
                var t = SDL.SDL_GetTicks();
                platformInfo.MouseClicked = false;

                UpdateMouse();

                while (SDL.SDL_PollEvent(out @event) == 1)
                {
                    OnEvent?.Invoke(@event.type, ref @event);
                    handlers[(int)@event.type]?.Invoke(@event.type,ref @event);
                    
                    switch (@event.type)
                    {
                        case SDL.SDL_EventType.SDL_KEYUP:
                            platformInfo.RaiseOnKeyUp(@event.key.keysym.sym);
                            break;
                        case SDL.SDL_EventType.SDL_WINDOWEVENT:
                            if (@event.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED){
                                UpdateWindow();
                                platformInfo.RaiseOnResize();
                            }
                            break;
                        case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                            platformInfo.MouseClicked = true;
                            platformInfo.RaiseOnClick();
                            break;
                        case SDL.SDL_EventType.SDL_QUIT:
                            logger.LogInformation("Received SDL_QUIT");
                            return;
                    }
                }

                app.Update();

                SDL.SDL_GL_SwapWindow(window);

                t = 16 - (t - SDL.SDL_GetTicks());
                if (t > 0) SDL.SDL_Delay(t);
            }
        }

        public void Subscribe(SDL.SDL_EventType ev, PlatformEventHandler<SDL.SDL_EventType, SDL.SDL_Event> handler) 
            => handlers[(int)ev] += handler;

        public void Unsubscribe(SDL.SDL_EventType ev, PlatformEventHandler<SDL.SDL_EventType, SDL.SDL_Event> handler) 
            => handlers[(int)ev] -= handler;
    }
}