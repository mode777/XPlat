using System.Numerics;
using GLES2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Core;

namespace XPlat.Graphics
{

    public class PhongMaterial : Material
    {
        private readonly Texture texture;
        public PhongMaterial(Texture texture, Uniform uniform = Uniform.AlbedoTexture)
        {
            this.texture = texture;
            Uniform = uniform;
        }


        public Uniform Uniform { get; set; }
        public float Metallic { get; set; } = 0;
        public float Roughness { get; set; } = 0.8f;



        public void ApplyToShader(Shader shader)
        {
            GL.ActiveTexture(GL.TEXTURE0);
            GL.BindTexture(GL.TEXTURE_2D, texture.GlTexture.Handle);
            shader.SetUniform(Uniform, 0);

            shader.SetUniform(Uniform.Material_Metallic, Metallic);
            shader.SetUniform(Uniform.Material_Roughness, Roughness);
        }
    }
}