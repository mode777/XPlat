using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.LuaScripting;

namespace XPlat.Engine
{

    [SceneTemplate("3d")]
    public class SceneConfiguration3d : SceneConfiguration {
        public SceneConfiguration3d(IPlatform platform) : base()
        {
            AddSubSystem(new BehaviourSubsystem());
            AddRenderPass(new RenderPass3d(platform));
        }
    }
}