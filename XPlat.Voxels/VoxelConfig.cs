using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.Engine;
using XPlat.LuaScripting;

namespace XPlat.Voxels
{
    [SceneTemplate("voxel")]
    public class VoxelConfig : SceneConfiguration
    {
        public VoxelConfig(IPlatform platform) : base()
        {
            AddSubSystem(new BehaviourSubsystem());
            AddRenderPass(new RenderPass3d(platform, "PIXEL_SCALE_UV", "ALPHA_AO"));
        }
    }
}