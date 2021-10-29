using GLES2;
using NanoVGDotNet;
using System.Drawing;

namespace net6test.samples
{
    public class Nvg : ISdlApp
    {
        private NVGcontext vg;
        private int font;
        private float fsize = 12;
        private readonly IPlatformInfo platform;

        public Nvg(IPlatformInfo platform)
        {
            this.platform = platform;

        }

        public void Init()
        {
            vg = new NVGcontext();
            GlNanoVG.nvgCreateGL(ref vg, (int)NVGcreateFlags.NVG_ANTIALIAS |
                        (int)NVGcreateFlags.NVG_STENCIL_STROKES);
            font = NanoVG.nvgCreateFont(vg, "sans", "assets/Roboto-Regular.ttf");
        }

        public void Update()
        {
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            NanoVG.nvgBeginFrame(vg, platform.RendererSize.Width, platform.RendererSize.Height, 1);

            NanoVG.nvgBeginPath(vg);

            vg.Rect(20, 20, 200, 200);
            vg.Circle(100, 100, 20);
            vg.PathWinding((int)NVGsolidity.NVG_HOLE);
            vg.FillColor("#ff0000");
            vg.Fill();

            NanoVG.nvgEndFrame(vg);
            //throw new NotImplementedException();
        }
    }
}