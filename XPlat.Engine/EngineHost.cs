using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SDL2;
using XPlat.Core;
using XPlat.Engine.Serialization;

namespace XPlat.Engine
{

    public class EngineHost : ISdlApp
    {
        private readonly EngineConfiguration config;
        private readonly IPlatform platform;
        private readonly ISdlPlatformEvents events;
        private readonly IServiceProvider provider;
        private IServiceScope scope;
        private readonly ILogger<EngineHost> logger;
        private readonly SimpleFileWatcher watcher;
        private Scene scene;
        private bool sceneChanged = true;

        public EngineHost(ILogger<EngineHost> logger,
            IPlatform platform, 
            IConfiguration config, 
            ISdlPlatformEvents events,
            IServiceProvider provider)
        {
            this.logger = logger;
            this.events = events;
            this.provider = provider;
            this.platform = platform;
            this.config = config.GetSection("Engine").Get<EngineConfiguration>();
            //config.GetReloadToken()
            if(this.config.Debug){
                this.watcher = new SimpleFileWatcher(this.config.InitialScene);
                this.watcher.FileChanged += (a,b) => sceneChanged = true;
            }
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
            if (sceneChanged)
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
            scope?.Dispose();
            //resource.Scene?.Dispose();
            scope = provider.CreateScope();
            var reader = scope.ServiceProvider.GetRequiredService<SceneReader>();
            scene = reader.Read(config.InitialScene);
            scene.Init();
            sceneChanged = false;
        }

        private void UpdateRaw(){
            scene?.Update();
            scene?.Render();
        }

        private void OnError(Exception e){
            scene.Dispose();   
            logger.LogError(e.ToString());
        }
    }
}