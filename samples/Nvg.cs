using NanoVGDotNet;

namespace net6test.samples
{
    public class Nvg : ISdlApp
    {
        private NVGcontext ctx;
        private int font;
        private float fsize = 12;

        public void Init()
        {
            ctx = new NVGcontext();
            GlNanoVG.nvgCreateGL(ref ctx, (int)NVGcreateFlags.NVG_ANTIALIAS |
						(int)NVGcreateFlags.NVG_STENCIL_STROKES);
            font = NanoVG.nvgCreateFont(ctx, "sans", "assets/Roboto-Regular.ttf");
        }

        public void Update()
        {
            var size = SdlHost.Current.RendererSize;
            NanoVG.nvgBeginFrame(ctx, size.Width, size.Height, 1.0f);
            
            NanoVG.nvgBeginPath(ctx);
            NanoVG.nvgRoundedRect(ctx, 20, 20, size.Width-40, size.Height-40, 50);
            NanoVG.nvgFillPaint(ctx, NanoVG.nvgLinearGradient(ctx, 0,0,size.Width,size.Height,NanoVG.nvgRGBA(0,128,255,255), NanoVG.nvgRGBA(255,0,128,255)));
            NanoVG.nvgFill(ctx);
            
            NanoVG.nvgFontFace(ctx, "sans");
            NanoVG.nvgTextAlign(ctx, (int)NVGalign.NVG_ALIGN_CENTER | (int)NVGalign.NVG_ALIGN_CENTER);
            NanoVG.nvgFontSize(ctx, 100);
            NanoVG.nvgFontBlur(ctx, 30);
            NanoVG.nvgFillColor(ctx, NanoVG.nvgRGBA(0,0,0,255));
            NanoVG.nvgTextBox(ctx, 0, size.Height/2, size.Width, "Welcome to NanoVG");
            NanoVG.nvgFontBlur(ctx, 0);
            NanoVG.nvgFillColor(ctx, NanoVG.nvgRGBA(255,255,255,255));
            NanoVG.nvgTextBox(ctx, 0, size.Height/2, size.Width, "Welcome to NanoVG");
            
            NanoVG.nvgEndFrame(ctx);
            //throw new NotImplementedException();
        }
    }
}