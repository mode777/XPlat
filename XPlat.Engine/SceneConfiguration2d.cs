using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;

namespace XPlat.Engine
{
    [SceneTemplate("2d")]
    public class SceneConfiguration2d : SceneConfiguration {
        public SceneConfiguration2d(IServiceProvider services) : base(services)
        {
            var platform = services.GetService<IPlatform>();
            AddSubSystem(new BehaviourSubsystem());
            AddSubSystem(new Collision2dSubsystem());
            AddRenderPass(new RenderPass2d(platform));
            AddRenderPass(new CanvasRenderPass(platform));
            //AddRenderPass(new DebugRenderPass(platform));
        }
    }
}