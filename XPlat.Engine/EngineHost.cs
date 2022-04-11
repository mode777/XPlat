using Microsoft.Extensions.Configuration;
using SDL2;
using XPlat.Core;

namespace XPlat.Engine
{

    public class EngineHost : ISdlApp
    {
        private readonly AppConfiguration config;
        private readonly SceneResource resource;
        private readonly IPlatform platform;
        private readonly ISdlPlatformEvents events;
        private readonly IServiceProvider services;

        public EngineHost(IServiceProvider services, IPlatform platform, IConfiguration config, ISdlPlatformEvents events)
        {
            this.services = services;
            this.events = events;
            this.platform = platform;
            this.config = config.GetSection("App").Get<AppConfiguration>();
            //config.GetReloadToken()
            this.resource = new SceneResource(services, "_scene", this.config.InitialScene);

            events.Subscribe(SDL.SDL_EventType.SDL_KEYUP, OnKeyUp);

        }

        public void OnKeyUp(SDL.SDL_EventType type, ref SDL.SDL_Event ev)
        {
            if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_F5)
            {
                Init();
            }
        }

        public void Init()
        {
            resource.Load();
            resource.Scene.Init();
            if (config.Debug) resource.Watch();
        }

        public void Update()
        {
            if (resource.FileChanged)
            {
                Init();
            }
            resource.Scene.Update();
            resource.Scene.Render();
        }
    }
}