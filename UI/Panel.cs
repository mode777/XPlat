using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{

    public class Panel : Element
    {
        private BoxDrawParams _drawParams;

        public FillStyle? Fill { get; set; }
        public FillStyle? HoverFill { get; set; }
        public Quantity CornerRadius { get; set; }
        public Shadow? Shadow { get; set; }
        public Thickness Padding { get; set; }
        public RectQ Rect { get; set; }

        public override void Update(NVGcontext vg)
        {
            base.Update(vg);

            var fill = Fill;
            if(HasMouseOver && HoverFill != null)
                fill = HoverFill;

            _drawParams = new BoxDrawParams{
                Rect = Bounds,
                CornerRadius = CornerRadius,
                Fill = fill ?? "#000000",
                Shadow = Shadow != null
                    ? new ShadowDrawParams(vg, Bounds, CornerRadius, Shadow.Offset, Shadow.Size, Shadow.Color)
                    : null
            };
        }

        public override SizeF CalculateSize(UiContext ctx)
        {
            return new SizeF(Rect.Width, Rect.Height);
        }

        public override void Arrange(UiContext ctx){

            var size = CalculateSize(ctx);
            SetPixelSize(size.Width, size.Height);
            SetPixelPos(Rect.X, Rect.Y);

            var contextCopy = ctx.Clone();
            contextCopy.MaxX = size.Width - Padding.Right - Padding.Left;
            var y = Bounds.Y + Padding.Top;
            var x = Bounds.X + Padding.Left;

            foreach (var c in Children)
            {
                var sizeChild = c.CalculateSize(contextCopy);
                c.SetPixelSize(sizeChild.Width, sizeChild.Height);
                c.SetPixelPos(x, y);

                c.Arrange(contextCopy);
                y += sizeChild.Height;
            }
        }

        public override void Draw(NVGcontext vg)
        {
            if(Fill != null || HoverFill != null || Shadow != null)
                _drawParams.Draw(vg);
            base.Draw(vg);
        }

        // protected override SizeF CalculateSize(UiContext ctx) => new SizeF(Rect.Width, Rect.Height);

        // protected override PointF CalculatePos(UiContext ctx) => new PointF(Rect.X, Rect.Y);

    }
}