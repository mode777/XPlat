using System.Numerics;
using GLES2;
using SDL2;

namespace net6test.samples
{   
    public class SimpleCube : ISdlApp
    {
        private readonly IPlatform platform;

        public SimpleCube(IPlatform platform)
        {
            this.platform = platform;

        }
        float r = 0;

        public void Init()
        {
            shader = new Shader(File.ReadAllText("shaders/phong.vertex.glsl"), File.ReadAllText("shaders/phong.fragment.glsl"), new()
            {
                [StandardAttribute.Position] = "vertexPacked",
                [StandardAttribute.Normal] = "aNormal"
            }, new()
            {
                [StandardUniform.ModelMatrix] = "model",
                [StandardUniform.ViewMatrix] = "view",
                [StandardUniform.ProjectionMatrix] = "projection",
            });

            scene = new Scene();
            scene.RootNode.AddChild(CreateCubeNode());
            model = scene.FindNode("cube");
        }

        private Node CreateCubeNode(){
            var node = new Node { Name = "cube" };

            var desc = new VertexAttributeDescriptor(3, GL.FLOAT);
            var position = new VertexAttribute<float>(StandardAttribute.Position, positions, desc);
            desc = new VertexAttributeDescriptor(3, GL.FLOAT);
            var normal = new VertexAttribute<float>(StandardAttribute.Normal, vertexNormals, desc);
            var prim = new Primitive(new[] {position, normal}, new VertexIndices(indices));

            var mesh = new Mesh(new[] { prim });

            var component = new RendererComponent();
            component.Mesh = mesh;

            node.AddComponent(component);

            return node;
        }   

        public void Update(){
            DrawScene();
        }

        private void DrawScene()
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
    }
}