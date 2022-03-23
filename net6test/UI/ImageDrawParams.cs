using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public struct ImageDrawParams
    {
        public ImageDrawParams(NVGcontext vg, NvgImage image, RectangleF fitRect, ImageFillStrategy strategy)
        {
            var r = strategy.Apply(image, fitRect);
            Paint = vg.ImagePattern(r.X, r.Y, r.Width, r.Height, 0, image.Handle, 1);
            Rect = strategy is ImageFillStrategy.FitFillStrategy ? r : fitRect;
        }

        public RectangleF Rect;
        public NVGpaint Paint;

        public void Draw(NVGcontext vg){
            vg.BeginPath();
            vg.Rect(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            vg.FillPaint(Paint);
            vg.Fill();
        }
    }
}
