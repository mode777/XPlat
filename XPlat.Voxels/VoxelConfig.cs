using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.Engine;

namespace XPlat.Voxels
{
    [SceneTemplate("voxel")]
    public class VoxelConfig : SceneConfiguration
    {
        public VoxelConfig(IServiceProvider services) : base(services)
        {
            AddSubSystem(new BehaviourSubsystem());
            AddRenderPass(new RenderPass3d(services.GetService<IPlatform>(), "PIXEL_SCALE_UV", "ALPHA_AO"));
        }
    }
}