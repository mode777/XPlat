using GLES2;
using NanoVGDotNet;
using SDL2;
using System.Numerics;

namespace net6test.samples
{

    public class GltfLoading : ISdlApp
    {
        private readonly IPlatform platform;

        public GltfLoading(IPlatform platform)
        {
            this.platform = platform;

        }
        float r = 0;

        public Scene LoadScene()
        {
            var model = SharpGLTF.Schema2.ModelRoot.Load("assets/models/monkey.glb");
            return GltfLoader.LoadScene(model);
        }

        public void Init()
        {
            shader = new Shader(File.ReadAllText("shaders/phong.vertex.glsl"), File.ReadAllText("shaders/phong.fragment.glsl"), new()
            {
                [StandardAttribute.Position] = "aPos",
                [StandardAttribute.Normal] = "aNormal"
            }, new()
            {
                [StandardUniform.ModelMatrix] = "model",
                [StandardUniform.ViewMatrix] = "view",
                [StandardUniform.ProjectionMatrix] = "projection",
            });

            scene = LoadScene();
            model = scene.FindNode("Suzanne");

            vg = new NVGcontext();
            GlNanoVG.nvgCreateGL(ref vg, (int)NVGcreateFlags.NVG_ANTIALIAS |
                        (int)NVGcreateFlags.NVG_STENCIL_STROKES);

        }

        public void Update(){
            Draw3d();
            Draw2d();
        }

        private void Draw2d(){
            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);
            vg.BeginPath();
            vg.Rect(20,20,800,1800);
            vg.FillColor("#ff000088");
            vg.Fill();
            vg.EndFrame();
        }

        private void Draw3d()
        {
            GL.UseProgram(shader.Handle);
            Shader.Use(shader);

            GL.Enable(GL.DEPTH_TEST);
            GL.Enable(GL.CULL_FACE);

            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

            var screenSize = platform.RendererSize;
            matP = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 4, screenSize.Width / (float)screenSize.Height, 0.1f, 100);
            shader.SetUniform(StandardUniform.ProjectionMatrix, ref matP);

            var cameraPos = new Vector3(0.001f, 7, 0);
            var cameraTarget = new Vector3(0, 0, 0);
            matV = Matrix4x4.CreateLookAt(cameraPos, cameraTarget, new Vector3(0, 1, 0));
            shader.SetUniform(StandardUniform.ViewMatrix, ref matV);
            shader.SetUniform("viewPos", cameraPos);
            var m = platform.MousePosition;
            var mx = (m.X / (float)platform.RendererSize.Width) * 10.0f - 5.0f;
            var my = (m.Y / (float)platform.RendererSize.Height) * 10.0f - 5.0f;
            shader.SetUniform("lightPos", new Vector3(my,4,-mx));
            shader.SetUniform("lightColor", "#ffffff");
            shader.SetUniform("objectColor", "#ffffff");

            float time = SDL.SDL_GetTicks() / 1000f;
            var scale = (float)Math.Sin(time) / 4 + 1;
            model.Transform.Scale = new Vector3(scale, scale, scale);
            model.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(r += 0.01f, 0, r + (float)Math.PI);
            matM = model.Transform.GetMatrix();
            //shader.SetUniform(StandardUniform.ModelMatrix, ref matM);


            Matrix4x4 matMI;
            var succ = Matrix4x4.Invert(matM, out matMI);
            Matrix4x4 matMIT = Matrix4x4.Transpose(matMI);
            shader.SetUniform("modelTransInv", ref matMIT);

            scene.RootNode.Draw();
        }
        private Shader shader;
        private Matrix4x4 matP;
        private Matrix4x4 matV;
        private Matrix4x4 matM;
        private Scene scene;
        private Node model;
        private NVGcontext vg;
    }
}