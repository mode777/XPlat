using NanoVGDotNet;

namespace net6test.UI
{
    public class ColorFillStyle : FillStyle {
        public NVGcolor Color;

        public ColorFillStyle(NVGcolor color)
        {
            Color = color;
        }

        public override void Apply(NVGcontext vg)
        {
            vg.FillColor(Color);
        }
    }
}
