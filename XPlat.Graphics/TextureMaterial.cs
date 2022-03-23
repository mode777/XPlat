using System.Numerics;
using GLES2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Core;

namespace XPlat.Graphics
{

    public class TextureMaterial : Material
    {
        private readonly Texture texture;
        public TextureMaterial(Texture texture, Uniform uniform = Uniform.AlbedoTexture)
        {
            this.texture = texture;
            Uniform = uniform;
        }


        public Uniform Uniform { get; set; }



        public void ApplyToShader(Shader shader)
        {
            GL.ActiveTexture(GL.TEXTURE0);
            GL.BindTexture(GL.TEXTURE_2D, texture.Handle);
            shader.SetUniform(Uniform, 0);
        }
    }
}