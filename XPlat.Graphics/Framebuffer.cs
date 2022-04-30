using GLES2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using XPlat.Core;

namespace XPlat.Graphics
{
    [Flags]
    public enum FramebufferAttachments
    {
        Color = 1,
        Depth = 2,
        Stencil = 4
    }

    public class Framebuffer
    {
        private readonly Texture texture;
        private readonly GlFramebufferHandle handle;

        public Size Size => texture.Size;
        public Texture Texture => texture;

        public Framebuffer(Texture texture, FramebufferAttachments attachments = FramebufferAttachments.Color)
        {
            this.texture = texture;
            handle = GlUtil.CreateFramebuffer(texture.GlTexture);
        }

        public void Draw(Action fn)
        {
            var vp = GlUtil.GetViewport();
            GL.BindFramebuffer(GL.FRAMEBUFFER, handle.Handle);
            GL.Viewport(0, 0, (uint)texture.Width, (uint)texture.Height);
            GL.ClearColor(0, 0, 0, 0.0f);
            GL.Clear(GL.COLOR_BUFFER_BIT);
            fn();
            GL.BindFramebuffer(GL.FRAMEBUFFER, 0);
            GL.Viewport(vp[0], vp[1], (uint)vp[2], (uint)vp[3]);
        }
    }
}
