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

    public class Texture : IDisposable
    {
        private bool disposedValue;

        public Vector2 Size { get; private set; }

        public Texture(Image<Rgba32> image, TextureUsage usage = TextureUsage.Graphics3d)
        {
            Size = new Vector2(image.Width, image.Height);
            Handle = GlUtil.CreateTexture2d(image);
            SetProperties(usage);
        }

        public Texture(string path, TextureUsage usage = TextureUsage.Graphics3d)
            : this(Image.Load<Rgba32>(path))
        {

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

        public uint Handle { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                UnmanagedQueue.DeleteTextures.Enqueue(Handle);
                //GlUtil.DeleteTexture(Handle);
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Texture()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
