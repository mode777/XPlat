using GLES2;
using SDL2;
using System.Numerics;

namespace net6test.samples
{

    public class ShadedCube : ISdlApp
    {

        float r = 0;

        public Scene LoadScene()
        {
            var model = SharpGLTF.Schema2.ModelRoot.Load("assets/monkey.glb");
            return GltfLoader.LoadScene(model);
        }

        public void Init()
        {
            shader = new Shader(File.ReadAllText("shaders/phong.vertex.glsl"), File.ReadAllText("shaders/phong.fragment.glsl"), new()
            {
                [StandardAttribute.Position] = "position",
                [StandardAttribute.Normal] = "normal"
            }, new() 
            {
                [StandardUniform.ModelMatrix] = "meshTransform",
                [StandardUniform.ViewMatrix] = "cameraLookAt",
                [StandardUniform.ProjectionMatrix] = "cameraProjection",
            });
            Shader.Use(shader);

            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_CULL_FACE);

            scene = LoadScene();
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
            shader.SetUniform("cameraPosition", cameraPos);

            matM = Matrix4x4.CreateRotationY(r += 0.01f);
            shader.SetUniform(StandardUniform.ModelMatrix, ref matM);

            Matrix4x4 matMI;
            Matrix4x4.Invert(matM, out matMI);
            Matrix4x4 matMIT = Matrix4x4.Transpose(matMI);
            shader.SetUniform("meshTransformTransposedInverse", ref matMIT);

            scene.RootNode.Draw();
        }
        private Shader shader;
        private Matrix4x4 matP;
        private Matrix4x4 matV;
        private Matrix4x4 matM;
        private Scene scene;
    }
}