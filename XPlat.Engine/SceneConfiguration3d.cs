using XPlat.Core;

namespace XPlat.Engine
{
    public class SceneConfiguration3d : SceneConfiguration {
        public SceneConfiguration3d(IPlatform platform)
        {
            var bs = new BehaviourSubsystem();
            AddInitSystem(bs);
            AddUpdateSystem(bs);
            AddRenderPass(new RenderPass3d(platform));
        }
    }
}