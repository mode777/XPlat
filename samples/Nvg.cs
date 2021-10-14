using ExCSS;
using GLES2;
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
            GL.ClearColor(0,0,0,1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
            var windowSize = SdlHost.Current.WindowSize;
            var rendererSize = SdlHost.Current.RendererSize;
            var scalex = rendererSize.Width / (float)windowSize.Width;
            var scaley = rendererSize.Height / (float)windowSize.Height;

            

            NanoVG.nvgBeginFrame(ctx, rendererSize.Width, rendererSize.Height, 1);
            NanoVG.nvgScale(ctx, scalex, scaley);
            
            // NanoVG.nvgBeginPath(ctx);
            // NanoVG.nvgRoundedRect(ctx, 10, 10, windowSize.Width-20, windowSize.Height-20, 25);
            // NanoVG.nvgFillPaint(ctx, NanoVG.nvgLinearGradient(ctx, 0,0,windowSize.Width,windowSize.Height,NanoVG.nvgRGBA(0,128,255,255), NanoVG.nvgRGBA(255,0,128,255)));
            // NanoVG.nvgFill(ctx);
            
            // NanoVG.nvgFontFace(ctx, "sans");
            // NanoVG.nvgTextAlign(ctx, (int)NVGalign.NVG_ALIGN_CENTER | (int)NVGalign.NVG_ALIGN_CENTER);
            // NanoVG.nvgFontSize(ctx, 50);
            // NanoVG.nvgFontBlur(ctx, 15);
            // NanoVG.nvgFillColor(ctx, NanoVG.nvgRGBA(0,0,0,255));
            // NanoVG.nvgTextBox(ctx, 0, windowSize.Height/2, windowSize.Width, "Welcome to NanoVG");
            // NanoVG.nvgFontBlur(ctx, 0);
            // NanoVG.nvgFillColor(ctx, NanoVG.nvgRGBA(255,255,255,255));
            // NanoVG.nvgTextBox(ctx, 0, windowSize.Height/2, windowSize.Width, "Welcome to NanoVG");

            NanoVG.nvgBeginPath(ctx);
            NanoVG.nvgRect(ctx, 0,0, windowSize.Width, windowSize.Height);
            NanoVG.nvgFillColor(ctx, NanoVG.nvgRGBA(250, 249, 248, 255));
            NanoVG.nvgFill(ctx);
            
            ctx.DrawBoxShadow(20,20,100,100,2);

            NanoVG.nvgEndFrame(ctx);
            //throw new NotImplementedException();
        }
    }
}