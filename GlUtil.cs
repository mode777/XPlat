
using System.Text;
using GLES2;
namespace net6test
{

    public static class GlUtil
    {
        public static uint CreateBuffer<T>(uint buffer_type, T[] data) where T : unmanaged {
            uint[] buffer = new uint[1];
            unsafe{ 
                fixed(uint* p = buffer){
                    GL.glGenBuffers(1, p);
                }
                GL.glBindBuffer(buffer_type, buffer[0]);
                fixed(void* p = data){
                    GL.glBufferData(buffer_type, (uint)(sizeof(T)*data.Length), p, GL.GL_STATIC_DRAW);
                }
            }
            return buffer[0];            
        }

        public static uint CreateProgram(string vertex_src, string fragment_src){
            var vshader = CreateShader(GL.GL_VERTEX_SHADER, vertex_src);
            var fshader = CreateShader(GL.GL_FRAGMENT_SHADER, fragment_src);

            var program = GL.glCreateProgram();
            GL.glAttachShader(program, vshader);
            GL.glAttachShader(program, fshader);
            GL.glLinkProgram(program);

            GL.glDeleteShader(vshader);
            GL.glDeleteShader(fshader);
            
            int success;
            GL.glGetProgramiv(program, GL.GL_LINK_STATUS, out success);
            if(success == 0){
                var sb = new StringBuilder(1024);
                uint length;
                GL.glGetProgramInfoLog(program, (uint)sb.Capacity, out length, sb);
                GL.glDeleteProgram(program);
                throw new Exception("Link program error: " + sb.ToString(0, (int)length));
            }
  
            return program;
        }

        private static uint CreateShader(uint shader_type, string shader_src)
        {
            uint shader = GL.glCreateShader(shader_type);
            
            int[] lengths = { shader_src.Length };
            string[] strings = { shader_src };

            unsafe { fixed(int* p = lengths) {
                GL.glShaderSource(shader, 1, strings, p);
                GL.glCompileShader(shader);
            } };
            
            int success;
            GL.glGetShaderiv(shader, GL.GL_COMPILE_STATUS, out success);
            if(success == 0){
                var sb = new StringBuilder(1024);
                uint length;
                GL.glGetShaderInfoLog(shader, (uint)sb.Capacity, out length, sb);
                GL.glDeleteShader(shader);
                throw new Exception("Compile shader error: " + sb.ToString(0, (int)length));
            }
            return shader;
        }
    }
}