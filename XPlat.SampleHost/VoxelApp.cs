using System.Numerics;
using CsharpVoxReader;
using XPlat.Core;
using XPlat.Engine;
using XPlat.Engine.Components;
using XPlat.Graphics;
using XPlat.Voxels;
using Attribute = XPlat.Graphics.Attribute;

namespace XPlat.SampleHost
{

    public class VoxelApp : ISdlApp
    {
        private Scene scene;
        private readonly IServiceProvider services;
        private readonly IPlatform platform;

        public VoxelApp(IPlatform platform, IServiceProvider services)
        {
            platform.ClearColor = new Vector3(1,0,0);
            this.services = services;
            this.platform = platform;
        }

        public void Init()
        {
            var config = new SceneConfiguration();
            config.AddSubSystem(new BehaviourSubsystem());
            config.AddRenderPass(new RenderPass3d(platform, "PIXEL_SCALE_UV"));
            scene = new Scene(config);

            var camera = new Node(scene)
            {
                Tag = "camera",
                Name = "Cam",
                Transform = new Transform3d
                {
                    TranslationVector = new Vector3(0, 15, -30),
                    RotationQuat = Quaternion.CreateFromYawPitchRoll(3, -0.5f, 0)
                }
            };
            camera.AddComponent(new CameraComponent());
            scene.RootNode.AddChild(camera);

            var cube = new Node(scene)
            {
                Name = "Vox"
            };
            cube.AddComponent(new MeshComponent
            {
                Mesh = new Mesh(LoadVox("assets/models/monastery.vox"))
            });
            float r = 0;
            cube.AddComponent(new ActionComponent(null, c =>
            {
                if(Input.IsKeyDown(Key.SPACE))
                {
                    r+=0.03f;
                    c.Node.Transform.RotationQuat = Quaternion.CreateFromYawPitchRoll(r/2, 0, 0);
                }
            }));
            scene.RootNode.AddChild(cube);

            var light = new Node(scene)
            {
                Name = "Light",
                Transform = new Transform3d
                {
                    TranslationVector = new Vector3(30, 20, -20)
                }
            };
            light.AddComponent(new LightComponent{
                Light = new PointLight {
                    Intensity = 100
                }
            });
            scene.RootNode.AddChild(light);
        }

        private Primitive LoadVox(string filename){
            var loader = new VoxLoader();
            var r = new VoxReader(filename, loader);
            r.Read();
            
            var prim = loader.GetPrimitive();
            //prim.Material = loader.GetMaterial();
            return prim;
        }

        public void Update()
        {
            scene.Update();
            scene.Render();
        }
    }
}