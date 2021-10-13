using NanoVGDotNet;

namespace net6test.samples
{
    public class Nvg : ISdlApp
    {
        private NVGcontext ctx;

        public void Init()
        {
            ctx = new NVGcontext();
            GlNanoVG.nvgCreateGL(ref ctx, (int)NVGcreateFlags.NVG_ANTIALIAS |
						(int)NVGcreateFlags.NVG_STENCIL_STROKES);
        }

        public void Update()
        {
            var size = SdlHost.Current.RendererSize;
            NanoVG.nvgBeginFrame(ctx, size.Width, size.Height, 1.0f);
            NanoVG.nvgBeginPath(ctx);
            NanoVG.nvgRoundedRect(ctx, 20, 20, size.Width-40, size.Height-40, 50);
            NanoVG.nvgFillColor(ctx, NanoVG.nvgRGBA(255, 192, 0, 255));
            NanoVG.nvgFill(ctx);
            NanoVG.nvgEndFrame(ctx);
            //throw new NotImplementedException();
        }
    }
}