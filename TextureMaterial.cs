using GLES2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace net6test
{
    public class TextureMaterial : Material
    {
        public TextureMaterial(string path, StandardUniform uniform)
        {
            Texture = GlUtil.CreateTexture2d(Image.Load<Rgba32>(path));
            Uniform = uniform;
        }

        public TextureMaterial(uint texture, StandardUniform uniform)
        {
            Texture = texture;
            Uniform = uniform;
        }

        public uint Texture { get; set; }

        public StandardUniform Uniform { get; set; }

        

        public override void ApplyToShader(Shader shader)
        {
            GL.ActiveTexture(GL.TEXTURE0);
            GL.BindTexture(GL.TEXTURE_2D, Texture);
            shader.SetUniform(Uniform, 0);
        }
    }
}