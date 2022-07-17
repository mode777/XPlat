using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;

namespace XPlat.Engine
{

    [SceneTemplate("3d")]
    public class SceneConfiguration3d : SceneConfiguration {
        public SceneConfiguration3d(IServiceProvider services) : base(services)
        {
            AddSubSystem(new BehaviourSubsystem());
            AddRenderPass(new RenderPass3d(Services.GetService<IPlatform>()));
        }
    }
}