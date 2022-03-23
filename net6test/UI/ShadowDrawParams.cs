using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public struct ShadowDrawParams
    {
        public RectangleF Rect;
        public NVGpaint Paint;

        public ShadowDrawParams(NVGcontext vg, RectangleF caster, float casterCorner, PointF offset, float size, NVGcolor color)
        {
            Rect = caster.Expand(size);
            Rect.Offset(offset);

            var trans = color;
            trans.a = 0;
            Paint = vg.BoxGradient(
                x: caster.X,
                y: caster.Y,
                w: caster.Width,
                h: caster.Height,
                r: (size / 2f) + casterCorner,
                f: size,
                icol: color,
                ocol: trans);
        }
                        
        public void Draw(NVGcontext vg, RectangleF caster, float cornerRadius)
        {
            vg.BeginPath();
            vg.Rect(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            vg.PathWinding((int)NVGsolidity.NVG_HOLE);
            vg.RoundedRect(caster.X, caster.Y, caster.Width, caster.Height, cornerRadius);
            vg.FillPaint(Paint);
            vg.Fill();
        }
    }
}
