using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SDL2;
using XPlat.Core;

namespace XPlat.Engine
{

    public class EngineHost : ISdlApp
    {
        private readonly EngineConfiguration config;
        private readonly SceneResource resource;
        private readonly IPlatform platform;
        private readonly ISdlPlatformEvents events;
        private readonly IServiceProvider services;
        private readonly ILogger<EngineHost> logger;

        public EngineHost(ILogger<EngineHost> logger, 
            IServiceProvider services, 
            IPlatform platform, 
            IConfiguration config, 
            ISdlPlatformEvents events)
        {
            this.logger = logger;
            this.services = services;
            this.events = events;
            this.platform = platform;
            this.config = config.GetSection("Engine").Get<EngineConfiguration>();
            //config.GetReloadToken()
            this.resource = new SceneResource(services, "_scene", this.config.InitialScene);
            if (this.config.Debug) resource.Watch();

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
            TryExecute(InitRaw,OnError);
        }

        public void Update()
        {
            if (resource.FileChanged)
            {
                Init();
            }
            TryExecute(UpdateRaw,OnError);
        }

        private void TryExecute(Action code, Action<Exception> error){
            if(config.ThrowExceptions){
                code.Invoke();
            } else {
                try {
                    code.Invoke();
                } catch(Exception e){
                    error.Invoke(e);
                }
            }
        }

        private void InitRaw(){
            resource.Load();
            resource.Scene?.Init();
        }

        private void UpdateRaw(){
            resource.Scene?.Update();
            resource.Scene?.Render();
        }

        private void OnError(Exception e){
            resource.Unload();   
            logger.LogError(e.ToString());
        }
    }
}