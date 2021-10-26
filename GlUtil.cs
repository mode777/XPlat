
using System.Numerics;
using System.Text;
using GLES2;
using NanoVGDotNet;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace net6test
{

    public static class GlUtil
    {
        public static void SendUniform(int location, ref Matrix4x4 mat){
            unsafe { fixed(float* p = &mat.M11) {
                GL.UniformMatrix4fv(location, 1, false, p);
            } }
        }

        public static void SendUniform(int location, int v)
        {
            GL.Uniform1i(location, v);
        }

        public static void SendUniform(int location, float v){
            GL.Uniform1f(location, v);
        }

        public static void SendUniform(int location, Vector3 v3){
            unsafe {
                GL.Uniform3fv(location, 1, &v3.X);
            } 
        }

        public static void SendUniform(int location, NVGcolor c){
            unsafe {
                GL.Uniform4fv(location, 1, &c.r);
            } 
        }

        public static uint CreateBuffer<T>(uint buffer_type, T[] data) where T : unmanaged {
            uint[] buffer = new uint[1];
            unsafe{ 
                fixed(uint* p = buffer){
                    GL.GenBuffers(1, p);
                }
                GL.BindBuffer(buffer_type, buffer[0]);
                fixed(void* p = data){
                    GL.BufferData(buffer_type, (uint)(sizeof(T)*data.Length), p, GL.STATIC_DRAW);
                }
            }
            return buffer[0];            
        }

        public static uint CreateProgram(string vertex_src, string fragment_src){
            var vshader = CreateShader(GL.VERTEX_SHADER, vertex_src);
            var fshader = CreateShader(GL.FRAGMENT_SHADER, fragment_src);

            var program = GL.CreateProgram();
            GL.AttachShader(program, vshader);
            GL.AttachShader(program, fshader);
            GL.LinkProgram(program);

            GL.DeleteShader(vshader);
            GL.DeleteShader(fshader);
            
            int success;
            GL.GetProgramiv(program, GL.LINK_STATUS, out success);
            if(success == 0){
                var sb = new StringBuilder(1024);
                uint length;
                GL.GetProgramInfoLog(program, (uint)sb.Capacity, out length, sb);
                GL.DeleteProgram(program);
                throw new Exception("Link program error: " + sb.ToString(0, (int)length));
            }
  
            return program;
        }

        private static uint CreateShader(uint shader_type, string shader_src)
        {
            uint shader = GL.CreateShader(shader_type);
            
            int[] lengths = { shader_src.Length };
            string[] strings = { shader_src };

            unsafe { fixed(int* p = lengths) {
                GL.ShaderSource(shader, 1, strings, p);
                GL.CompileShader(shader);
            } };
            
            int success;
            GL.GetShaderiv(shader, GL.COMPILE_STATUS, out success);
            if(success == 0){
                var sb = new StringBuilder(1024);
                uint length;
                GL.GetShaderInfoLog(shader, (uint)sb.Capacity, out length, sb);
                GL.DeleteShader(shader);
                throw new Exception("Compile shader error: " + sb.ToString(0, (int)length));
            }
            return shader;
        }

        public static uint CreateTexture2d(Image<Rgba32> image)
        {
            unsafe
            {
                uint tex = 0;
                GL.GenTextures(1, &tex);
                GL.BindTexture(GL.TEXTURE_2D, tex);
                if(image.TryGetSinglePixelSpan(out var span))
                {
                    fixed(Rgba32* p = span)
                    {
                        GL.TexImage2D(GL.TEXTURE_2D, 0, (int)GL.RGBA, (uint)image.Width, (uint)image.Height, 0, GL.RGBA, GL.UNSIGNED_BYTE, p);
                    }
                    GL.GenerateMipmap(GL.TEXTURE_2D);
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, (int)GL.LINEAR);
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, (int)GL.LINEAR_MIPMAP_LINEAR);
                } else
                {
                    throw new Exception("Cannot access image pixels");
                }
                return tex;
            }
        }
    }
}