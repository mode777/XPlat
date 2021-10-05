using GLES2;
using SDL2;

namespace net6test
{
    public class SdlHost
    {
        private readonly ISdlApp app;
        public SdlHost(ISdlApp app)
        {
            this.app = app;
        }

        public void Run()
        {
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);

            SDL.SDL_SetHint("SDL_OPENGL_ES_DRIVER", "1");
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 2);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 0);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_ES);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_RED_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_GREEN_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_BLUE_SIZE, 8);

            var window = SDL.SDL_CreateWindow("SDL running on .NET 6.0", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, 640, 480,
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI);
            
            var context = SDL.SDL_GL_CreateContext(window);
            SDL.SDL_GL_SetSwapInterval(0);

            app.Init();
            
            SDL.SDL_Event @event;

            while(true){
                var t = SDL.SDL_GetTicks();
    
                while(SDL.SDL_PollEvent(out @event) == 1){
                    switch (@event.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            return;
                    }
                }
                int w,h;
                SDL.SDL_GL_GetDrawableSize(window, out w, out h);
                GL.glViewport(0,0,(uint)w,(uint)h);

                app.Update();

                SDL.SDL_GL_SwapWindow(window);
                
                t = 17-(t-SDL.SDL_GetTicks());
                if(t > 0) SDL.SDL_Delay(t);      
            }
        }

    }
}