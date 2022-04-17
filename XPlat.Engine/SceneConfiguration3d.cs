using XPlat.Core;

namespace XPlat.Engine
{
    public class SceneConfiguration3d : SceneConfiguration {
        public SceneConfiguration3d(IPlatform platform)
        {
            AddSubSystem(new BehaviourSubsystem());
            AddRenderPass(new RenderPass3d(platform));
        }
    }
}