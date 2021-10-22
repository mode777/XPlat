using System.Drawing;
using System.Numerics;
using GLES2;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SDL2;

namespace net6test
{
    public class SdlPlatform
    {
        private SDL.SDL_Event @event;
        private IntPtr window;
        private IntPtr context;
        private bool isRunning = false;
        private readonly ISdlApp app;
        private readonly ILogger<SdlPlatform> logger;
        private readonly IHostApplicationLifetime lifetime;
        private readonly PlatformInfo platformInfo;

        public SdlPlatform(ISdlApp app,
                           ILogger<SdlPlatform> logger,
                           IHostApplicationLifetime lifetime,
                           PlatformInfo platformInfo)
        {
            this.logger = logger;
            this.lifetime = lifetime;
            this.platformInfo = platformInfo;
            this.app = app;

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

            window = SDL.SDL_CreateWindow("SDL running on .NET 6.0", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 640, 480,
            SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
            logger.LogInformation("Window initialized");

            context = SDL.SDL_GL_CreateContext(window);
            SDL.SDL_GL_SetSwapInterval(0);
            logger.LogInformation("OpengGL Context created");

            UpdatePlatform();

            app.Init();
            logger.LogInformation("App initialized");
            isRunning = true;
        }

        private void UpdatePlatform()
        {
            int w, h;

            SDL.SDL_GL_GetDrawableSize(window, out w, out h);
            platformInfo.RendererSize = new Size(w, h);

            GL.Viewport(0, 0, (uint)w, (uint)h);

            SDL.SDL_GetWindowSize(window, out w, out h);
            platformInfo.WindowSize = new Size(w, h);

            SDL.SDL_GetMouseState(out w, out h);
            platformInfo.MousePosition = new Point((int)(w * platformInfo.RetinaScale),(int)(h * platformInfo.RetinaScale));

        }

        public void Run()
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

                UpdatePlatform();

                while (SDL.SDL_PollEvent(out @event) == 1)
                {
                    switch (@event.type)
                    {
                        case SDL.SDL_EventType.SDL_WINDOWEVENT:
                            if (@event.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED)
                                platformInfo.RaiseOnResize();
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

                //t = 16-(t-SDL.SDL_GetTicks());
                //if(t > 0) SDL.SDL_Delay(t);   
                SDL.SDL_Delay(10);
            }
        }

    }
}