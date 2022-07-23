using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.LuaScripting;
using XPlat.NanoVg;

namespace XPlat.Engine
{
    [SceneTemplate("2d")]
    public class SceneConfiguration2d : SceneConfiguration {
        public SceneConfiguration2d(IPlatform platform, NVGcontext vg) : base()
        {
            AddSubSystem(new BehaviourSubsystem());
            AddSubSystem(new Collision2dSubsystem());
            AddRenderPass(new RenderPass2d(platform));
            AddRenderPass(new CanvasRenderPass(platform, vg));
            //AddRenderPass(new DebugRenderPass(platform));
        }
    }
}