using System.Numerics;
using XPlat.Core;
using XPlat.Engine;
using XPlat.Engine.Components;
using XPlat.Graphics;

namespace XPlat.SampleHost
{
    public class EngineApp : ISdlApp
    {
        private Scene scene;
        private readonly IServiceProvider services;

        public EngineApp(IPlatform platform, IServiceProvider services)
        {
            this.services = services;
            this.platform = platform;
        }

        public void Init()
        {
            var config = new SceneConfiguration3d(services);
            scene = new Scene(config);

            var camera = new Node(scene)
            {
                Tag = "camera",
                Name = "Cam",
                Transform = new Transform3d
                {
                    TranslationVector = new Vector3(0, 3, -5),
                    RotationQuat = Quaternion.CreateFromYawPitchRoll(3, -0.5f, 0)
                }
            };
            camera.AddComponent(new CameraComponent());
            scene.RootNode.AddChild(camera);

            var cube = new Node(scene)
            {
                Name = "Cube"
            };
            cube.AddComponent(new MeshComponent
            {
                Mesh = new Mesh(new Primitive(new[]
                {
                    new VertexAttribute<float>(Graphics.Attribute.Position, positions, VertexAttributeDescriptor.Vec3f),
                    new VertexAttribute<float>(Graphics.Attribute.Normal, vertexNormals, VertexAttributeDescriptor.Vec3f),
                    new VertexAttribute<float>(Graphics.Attribute.Uv_0, uvs, VertexAttributeDescriptor.Vec2f),
                }, new VertexIndices(indices))
                {
                    Material = new PhongMaterial(new Texture("assets/textures/bricks.jpeg"), Uniform.AlbedoTexture)
                })
            });
            cube.AddComponent(new ActionComponent(null, c =>
            {
                c.Node.Transform.RotationQuat = Quaternion.CreateFromYawPitchRoll(Time.RunningTime, 0, 0);
            }));
            scene.RootNode.AddChild(cube);

            var light = new Node(scene)
            {
                Name = "Light",
                Transform = new Transform3d
                {
                    TranslationVector = new Vector3(3, 2, -2)
                }
            };
            light.AddComponent(new LightComponent());
            scene.RootNode.AddChild(light);
        }

        public void Update()
        {
            scene.Update();
            scene.Render();
        }

        private readonly float[] uvs = {

            // Front
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Back
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Top
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Bottom
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Right
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Left
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,
         };

        private readonly float[] vertexNormals = {
            // Front
            0.0f,  0.0f,  1.0f,
            0.0f,  0.0f,  1.0f,
            0.0f,  0.0f,  1.0f,
            0.0f,  0.0f,  1.0f,

            // Back
            0.0f,  0.0f, -1.0f,
            0.0f,  0.0f, -1.0f,
            0.0f,  0.0f, -1.0f,
            0.0f,  0.0f, -1.0f,

            // Top
            0.0f,  1.0f,  0.0f,
            0.0f,  1.0f,  0.0f,
            0.0f,  1.0f,  0.0f,
            0.0f,  1.0f,  0.0f,

            // Bottom
            0.0f, -1.0f,  0.0f,
            0.0f, -1.0f,  0.0f,
            0.0f, -1.0f,  0.0f,
            0.0f, -1.0f,  0.0f,

            // Right
            1.0f,  0.0f,  0.0f,
            1.0f,  0.0f,  0.0f,
            1.0f,  0.0f,  0.0f,
            1.0f,  0.0f,  0.0f,

            // Left
            -1.0f,  0.0f,  0.0f,
            -1.0f,  0.0f,  0.0f,
            -1.0f,  0.0f,  0.0f,
            -1.0f,  0.0f,  0.0f
         };

        private readonly float[] positions = {
            // Front face
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,

            // Back face
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,

            // Top face
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f, -1.0f,

            // Bottom face
            -1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            // Right face
            1.0f, -1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,

            // Left face
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,
        };

        private readonly ushort[] indices = {
            0,  1,  2,      0,  2,  3,    // front
            4,  5,  6,      4,  6,  7,    // back
            8,  9,  10,     8,  10, 11,   // top
            12, 13, 14,     12, 14, 15,   // bottom
            16, 17, 18,     16, 18, 19,   // right
            20, 21, 22,     20, 22, 23,   // left
        };
        private readonly IPlatform platform;
    }
}