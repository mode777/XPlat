using NanoVGDotNet;

namespace net6test.samples
{
    public static class NvgUtils
    {
        public static void DrawBoxShadow(this NVGcontext vg, float x, float y, float w, float h, float cornerRadius)
        {

            // Drop shadow
            var shadowPaint = NanoVG.nvgBoxGradient(vg, x,y+1, w,h, cornerRadius+1, 6, NanoVG.nvgRGBA(0,0,0,45), NanoVG.nvgRGBA(0,0,0,0));
            NanoVG.nvgBeginPath(vg);
            NanoVG.nvgRect(vg, x-5,y-5, w+10,h+15);
            NanoVG.nvgRoundedRect(vg, x,y, w,h, cornerRadius);
            NanoVG.nvgPathWinding(vg, (int)NVGsolidity.NVG_HOLE);
            NanoVG.nvgFillPaint(vg, shadowPaint);
            NanoVG.nvgFill(vg);

            NanoVG.nvgBeginPath(vg);
            NanoVG.nvgRoundedRect(vg, x,y, w,h, cornerRadius);
            NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(255,255,255,255));
            NanoVG.nvgFill(vg);
        }
        
    }
}