using NanoVGDotNet;
using System.Drawing;

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
            var size = SdlHost.Current.WindowSize;
            var scale = new Size(SdlHost.Current.RendererSize.Width / size.Width, SdlHost.Current.RendererSize.Height / size.Height);

            NanoVG.nvgBeginFrame(ctx, size.Width, size.Height, 1);
            
            NanoVG.nvgBeginPath(ctx);
            NanoVG.nvgScale(ctx, scale.Width, scale.Height);
            NanoVG.nvgRoundedRect(ctx, 10, 10, size.Width-20, size.Height-20, 25);
            NanoVG.nvgFillPaint(ctx, NanoVG.nvgLinearGradient(ctx, 0,0,size.Width,size.Height,NanoVG.nvgRGBA(0,128,255,255), NanoVG.nvgRGBA(255,0,128,255)));
            NanoVG.nvgFill(ctx);
            
            NanoVG.nvgFontFace(ctx, "sans");
            NanoVG.nvgTextAlign(ctx, (int)NVGalign.NVG_ALIGN_CENTER | (int)NVGalign.NVG_ALIGN_CENTER);
            NanoVG.nvgFontSize(ctx, 50);
            NanoVG.nvgFontBlur(ctx, 15);
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