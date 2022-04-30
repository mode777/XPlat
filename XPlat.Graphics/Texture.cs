using System;
using System.Numerics;
using GLES2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Core;

namespace XPlat.Graphics
{
    public enum TextureUsage
    {
        Graphics3d,
        Graphics2d,
        GraphicsPixel
    }

    public class Texture
    {
        private bool disposedValue;

        public System.Drawing.Size Size { get; private set; }
        public int Width => Size.Width;
        public int Height => Size.Height;

        public Texture(Image<Rgba32> image, TextureUsage usage = TextureUsage.Graphics3d)
        {
            Size = new System.Drawing.Size(image.Width, image.Height);
            GlTexture = GlUtil.CreateTexture2d(image);
            SetProperties(usage);
        }

        public Texture(string path, TextureUsage usage = TextureUsage.Graphics3d)
            : this(Image.Load<Rgba32>(path))
        {

        }

        public Texture(int width, int height, TextureUsage usage = TextureUsage.Graphics3d)
        {
            Size = new System.Drawing.Size(width, height);
            GlTexture = GlUtil.CreateTexture2d(width, height);
            SetProperties(usage);
        }

        public void Update(Image<Rgba32> image, int x = 0, int y = 0)
        {
            GlUtil.UpdateTexture2d(GlTexture, x, y, image);
        }

        private void SetProperties(TextureUsage usage)
        {
            switch (usage)
            {
                case TextureUsage.Graphics3d:
                    GL.GenerateMipmap(GL.TEXTURE_2D);
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, (int)GL.LINEAR);
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, (int)GL.LINEAR_MIPMAP_LINEAR);
                    break;
                case TextureUsage.Graphics2d:
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, (int)GL.LINEAR);
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, (int)GL.LINEAR);
                    break;
                case TextureUsage.GraphicsPixel:
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, (int)GL.NEAREST);
                    GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, (int)GL.NEAREST);
                    break;
                default:
                    break;
            }
        }

        public GlTextureHandle GlTexture { get; }

        
    }
}
