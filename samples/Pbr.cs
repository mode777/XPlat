using GLES2;
using SDL2;
using System.Numerics;

namespace net6test.samples
{

    public class Pbr : ISdlApp
    {

        float r = 0;
        Transform t = new Transform();

        public Scene LoadScene()
        {
            var model = SharpGLTF.Schema2.ModelRoot.Load("assets/monkey.glb");
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

            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_CULL_FACE);

            scene = LoadScene();
            var model = scene.FindNode("Suzanne");

        }

        public void Update()
        {
            GL.glClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            var screenSize = SdlHost.Current.RendererSize;
            matP = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 2, screenSize.Width / (float)screenSize.Height, 0.1f, 100);
            shader.SetUniform(StandardUniform.ProjectionMatrix, ref matP);

            var cameraPos = new Vector3(1.5f, 0, 1.5f);
            var cameraTarget = new Vector3(0, 0, 0);
            matV = Matrix4x4.CreateLookAt(cameraPos, cameraTarget, new Vector3(0, 1, 0));
            shader.SetUniform(StandardUniform.ViewMatrix, ref matV);
            
            shader.SetUniform("albedo", new Vector3(1.0f,0,0));
            shader.SetUniform("metallic", 0.1f);
            shader.SetUniform("roughness", 0.5f);
            shader.SetUniform("ao", 1f);
            shader.SetUniform("lightPositions[0]", new Vector3(2,2,0));
            shader.SetUniform("lightColors[0]", new Vector3(1,1,1));
            shader.SetUniform("camPos", cameraPos);

            float time = SDL.SDL_GetTicks() / 1000f;
            var scale = (float)Math.Sin(time)/4+1;
            t.Scale = new Vector3(scale, scale, scale);
            t.Rotation = new Vector3(r+=0.01f,0,r+(float)Math.PI);
            matM = t.GetMatrix();
            //matM = Matrix4x4.CreateRotationY(r += 0.01f);
            shader.SetUniform(StandardUniform.ModelMatrix, ref matM);

            scene.RootNode.Draw();
        }
        private Shader shader;
        private Matrix4x4 matP;
        private Matrix4x4 matV;
        private Matrix4x4 matM;
        private Scene scene;
    }
}