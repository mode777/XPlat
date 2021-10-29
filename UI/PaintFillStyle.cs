using NanoVGDotNet;

namespace net6test.UI
{
    public class PaintFillStyle : FillStyle {
        public NVGpaint Paint;

        public PaintFillStyle(NVGpaint paint)
        {
            Paint = paint;
        }

        public override void Apply(NVGcontext vg)
        {
            vg.FillPaint(Paint);
        }
    }
}
