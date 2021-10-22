using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class Panel : Element
    {
        private BoxDrawParams _drawParams;

        public Panel()
        {
            Style = new Style();
            HoverStyle = new Style(Style);
        }

        public Style Style { get; }
        public Style HoverStyle { get; }
        public RectQ Rect { get; set; }

        public override void Update(NVGcontext vg)
        {
            base.Update(vg);

            var style = HasMouseOver ? Style : HoverStyle;

            _drawParams = new BoxDrawParams{
                Rect = Bounds,
                CornerRadius = style.CornerRadius ?? 0,
                Fill = style.Fill ?? "#000000",
                Shadow = style.Shadow != null
                    ? new ShadowDrawParams(vg, Bounds, style.CornerRadius ?? 0, Style.Shadow.Offset, Style.Shadow.Size, Style.Shadow.Color)
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
            contextCopy.MaxX = size.Width - Style.Padding?.Right - Style.Padding?.Left;
            var y = Bounds.Y + Style.Padding?.Top ?? 0;
            var x = Bounds.X + Style.Padding?.Left ?? 0;

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
            _drawParams.Draw(vg);
            base.Draw(vg);
        }

        // protected override SizeF CalculateSize(UiContext ctx) => new SizeF(Rect.Width, Rect.Height);

        // protected override PointF CalculatePos(UiContext ctx) => new PointF(Rect.X, Rect.Y);

    }
}