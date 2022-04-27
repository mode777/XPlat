//using OpenToolkit.Graphics.OpenGL;

//namespace Gwen.Net.OpenTk.Shaders
//{
//    public class GLShader : IShader
//    {
//        public int Program { get; }
//        public int VertexShader { get; }
//        public int FragmentShader { get; }
//        public UniformDictionary Uniforms { get; }

//        public GLShader(int program, int vertexShader, int fragmentShader)
//        {
//            Program = program;
//            VertexShader = vertexShader;
//            FragmentShader = fragmentShader;
//            Uniforms = new UniformDictionary(program, GL.GetUniformLocation);
//        }

//        public void Dispose()
//        {
//            GL.DeleteProgram(Program);
//        }
//    }
//}