using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public struct BoxDrawParams
    {
        public RectangleF Rect;
        public ShadowDrawParams? Shadow;
        public FillStyle Fill;
        public float CornerRadius;

        public void Draw(NVGcontext vg)
        {
            Shadow?.Draw(vg, Rect, CornerRadius);

            vg.BeginPath();
            Fill.Apply(vg);
            vg.RoundedRect(Rect.X, Rect.Y, Rect.Width, Rect.Height, CornerRadius);
            vg.Fill();
        }        
    }
}
