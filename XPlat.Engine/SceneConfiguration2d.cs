using XPlat.Core;

namespace XPlat.Engine
{
    public class SceneConfiguration2d : SceneConfiguration {
        public SceneConfiguration2d(IPlatform platform)
        {
            AddSubSystem(new BehaviourSubsystem());
            AddSubSystem(new Collision2dSubsystem());
            AddRenderPass(new RenderPass2d(platform));
            AddRenderPass(new DebugRenderPass(platform));
        }
    }
}