using NanoVGDotNet;

namespace net6test.UI
{
    public class GradientFileStyle : FillStyle {
        public NVGpaint Paint;

        public GradientFileStyle(NVGpaint paint)
        {
            Paint = paint;
        }

        public override void Apply(NVGcontext vg)
        {
            vg.FillPaint(Paint);
        }
    }
}
