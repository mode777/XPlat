using GLES2;
using SDL2;
using System.Numerics;

namespace net6test.samples
{

    public class Pbr : ISdlApp
    {
        private readonly IPlatformInfo platform;

        public Pbr(IPlatformInfo platform)
        {
            this.platform = platform;

        }

        float r = 0;

        public Scene LoadScene()
        {
            var model = SharpGLTF.Schema2.ModelRoot.Load("assets/scene.glb");
            return GltfLoader.LoadScene(model);
        }

        public void Init()
        {
            var vertexSource = File.ReadAllText("shaders/pbr.vertex.glsl");
            var fragmentSource = File.ReadAllText("shaders/pbr.fragment.glsl");
            shader = new Shader(vertexSource, fragmentSource, new()
            {
                [StandardAttribute.Position] = "aPos",
                [StandardAttribute.Normal] = "aNormal"
            }, new()
            {
                [StandardUniform.ModelMatrix] = "model",
                [StandardUniform.ViewMatrix] = "view",
                [StandardUniform.ProjectionMatrix] = "projection",
            });
            Shader.Use(shader);

            GL.Enable(GL.DEPTH_TEST);
            GL.Enable(GL.CULL_FACE);

            scene = LoadScene();
            var model = scene.FindNode("Suzanne");
            model?.AddComponent(new ActionComponent(null, (c) =>
            {
                float time = SDL.SDL_GetTicks() / 1000f;
                var scale = (float)Math.Sin(time) / 4 + 1;
                c.Transform.Scale = new Vector3(scale, scale, scale);
                c.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(r += 0.01f, 0, r + (float)Math.PI);
            }));

        }

        public void Update()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

            scene.Update();

            var screenSize = platform.RendererSize;
            matP = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 2, screenSize.Width / (float)screenSize.Height, 0.1f, 100);
            shader.SetUniform(StandardUniform.ProjectionMatrix, ref matP);

            var cameraPos = new Vector3(8f, 3, 3f);
            var cameraTarget = new Vector3(0, 0, 0);
            matV = Matrix4x4.CreateLookAt(cameraPos, cameraTarget, new Vector3(0, 1, 0));
            shader.SetUniform(StandardUniform.ViewMatrix, ref matV);

            shader.SetUniform("lightPositions[0]", new Vector3(2, 2, 0));
            shader.SetUniform("lightColors[0]", new Vector3(1, 1, 1));
            shader.SetUniform("camPos", cameraPos);

            scene.Draw();
        }
        private Shader shader;
        private Matrix4x4 matP;
        private Matrix4x4 matV;
        private Matrix4x4 matM;
        private Scene scene;
    }
}