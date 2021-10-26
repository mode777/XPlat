using GLES2;
using NanoVGDotNet;
using SDL2;
using System.Numerics;

namespace net6test.samples
{

    public class Lightmap : ISdlApp
    {
        private readonly IPlatformInfo platform;

        public Lightmap(IPlatformInfo platform)
        {
            this.platform = platform;

        }

        float r = 0;
        Vector3 lights = Vector3.Zero;

        public Scene LoadScene()
        {
            var model = SharpGLTF.Schema2.ModelRoot.Load("assets/night.glb");
            return GltfLoader.LoadScene(model);
        }

        public void Init()
        {
            var vertexSource = File.ReadAllText("shaders/lightmap.vertex.glsl");
            var fragmentSource = File.ReadAllText("shaders/lightmap.fragment.glsl");
            shader = new Shader(vertexSource, fragmentSource, new()
            {
                [StandardAttribute.Position] = "aPos",
                [StandardAttribute.Normal] = "aNormal",
                [StandardAttribute.Uv_0] = "aUv"
            }, new()
            {
                [StandardUniform.ModelMatrix] = "model",
                [StandardUniform.ViewMatrix] = "view",
                [StandardUniform.ProjectionMatrix] = "projection",
                [StandardUniform.LightmapTexture] = "texture"
            });
            Shader.Use(shader);

            GL.Enable(GL.DEPTH_TEST);
            GL.Enable(GL.CULL_FACE);

            scene = LoadScene();

            platform.OnKeyUp += (s, k) =>
            {
                if (k == SDL.SDL_Keycode.SDLK_1)
                    lights.X = lights.X > 0 ? 0 : 1;
                if (k == SDL.SDL_Keycode.SDLK_2)
                    lights.Y = lights.Y > 0 ? 0 : 1;
                if (k == SDL.SDL_Keycode.SDLK_3)
                    lights.Z = lights.Z > 0 ? 0 : 1;
            };
        }

        public void Update()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

            scene.Update();

            var screenSize = platform.RendererSize;
            matP = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 2, screenSize.Width / (float)screenSize.Height, 0.1f, 100);
            shader.SetUniform(StandardUniform.ProjectionMatrix, ref matP);

            v = (platform.MousePosition.Y / (float)platform.WindowSize.Height) * 10;
            var cameraPos = new Vector3(v, v, v);
            
            var cameraTarget = new Vector3(0, 0, 0);
            matV = Matrix4x4.CreateLookAt(cameraPos, cameraTarget, new Vector3(0, 1, 0));
            shader.SetUniform(StandardUniform.ViewMatrix, ref matV);

            var ticks = SDL.SDL_GetTicks();
            var sinVal = (float v) => (float)((Math.Sin(v) + 1.0) / 2.0);
            lights = new Vector3(sinVal(ticks), sinVal(ticks + 30), sinVal(ticks + 15));
            NVGcolor c = "#c4d2ff88";
            c.a = sinVal(ticks / 500f);
            NVGcolor c2 = "#ff441133";
            c2.a = sinVal(ticks / 120) * 0.3f + 0.1f;
            shader.SetUniform("lightColor0", c2);
            shader.SetUniform("lightColor1", c);
            shader.SetUniform("lightColor2", "#94f4ff22");
            shader.SetUniform("ambient", "#050008");

            scene.Draw();
        }
        private Shader shader;
        private Matrix4x4 matP;
        private Matrix4x4 matV;
        private Matrix4x4 matM;
        private Scene scene;
        private float v = 1;
    }
}