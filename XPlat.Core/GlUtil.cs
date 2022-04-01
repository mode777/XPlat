
using System;
using System.Numerics;
using System.Text;
using GLES2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace XPlat.Core
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

        public static void SendUniform(int location, Vector2 v2)
        {
            unsafe
            {
                GL.Uniform2fv(location, 1, &v2.X);
            }
        }

        public static void SendUniform(int location, Vector3 v3){
            unsafe {
                GL.Uniform3fv(location, 1, &v3.X);
            } 
        }

        public static void SendUniform(int location, Vector4 v4){
            unsafe {
                GL.Uniform4fv(location, 1, &v4.X);
            } 
        }

        //public static void SendUniform(int location, NVGcolor c){
        //    unsafe {
        //        GL.Uniform4fv(location, 1, &c.r);
        //    } 
        //}

        public static GlBufferHandle CreateBuffer<T>(uint buffer_type, T[] data, uint usageHint = GL.STATIC_DRAW) where T : unmanaged {
            uint buffer = 0;
            unsafe{ 
                GL.GenBuffers(1, &buffer);
                GL.BindBuffer(buffer_type, buffer);
                fixed(void* p = data){
                    GL.BufferData(buffer_type, (uint)(sizeof(T)*data.Length), p, usageHint);
                }
            }
            return new GlBufferHandle(buffer);            
        }

        public static void ResizeBuffer<T>(GlBufferHandle handle, uint buffer_type, T[] data, uint usageHint = GL.STATIC_DRAW) where T : unmanaged
        {
            unsafe
            {
                GL.BindBuffer(buffer_type, handle.Handle);
                fixed (void* p = data)
                {
                    GL.BufferData(buffer_type, (uint)(sizeof(T) * data.Length), p, usageHint);
                }
            }
        }

        public static void DeleteBuffer(uint buffer){
            unsafe {
                GL.DeleteBuffers(1, &buffer);
            }
        }

        public static void DeleteProgram(uint program){
            GL.DeleteProgram(program);
        }

        public static void DeleteTexture(uint texture){
            unsafe {
                GL.DeleteTextures(1, &texture);
            }
        }

        public static void UpdateBuffer<T>(GlBufferHandle buffer, uint bufferType, T[] data, int size = -1, int offset = -1) where T : unmanaged
        {
            unsafe
            {
                fixed(void* p = data)
                {
                    GL.BindBuffer(bufferType, buffer.Handle);
                    GL.BufferSubData(bufferType, (uint)(sizeof(T) * (offset == -1 ? 0 : offset)), (uint)(sizeof(T) * (size == -1 ? 0 : size)), p);
                }

            }
        }

        public static GlProgramHandle CreateProgram(string vertex_src, string fragment_src){
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
  
            return new GlProgramHandle(program);
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

        public static GlTextureHandle CreateTexture2d(Image<Rgba32> image)
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
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, (int)GL.CLAMP_TO_EDGE);
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, (int)GL.CLAMP_TO_EDGE);
                } else
                {
                    throw new Exception("Cannot access image pixels");
                }
                return new GlTextureHandle(tex);
            }
        }
    }
}