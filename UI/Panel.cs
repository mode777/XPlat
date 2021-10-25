using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class Panel : Element
    {
        private BoxDrawParams _drawParams;

        public Panel()
        {
            Style = new Style(Style.Default);
            HoverStyle = new Style(Style);
        }

        public Style Style { get; }
        public Style HoverStyle { get; }
        public Quantity? X { get; set; }
        public Quantity? Y { get; set; }
        public Quantity? Width { get; set; }
        public Quantity? Height { get; set; }
        public Thickness Padding { get; set; }

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
            return new SizeF(Width ?? ctx.MaxW ?? 0, Height ?? ctx.MaxH ?? 0);
        }

        public override void Arrange(UiContext ctx){
            SetPixelPos(X ?? ctx.X ?? 0, Y ?? ctx.Y ?? 0);
            SetPixelSize(Width ?? ctx.MaxW ?? 0, Height ?? ctx.MaxH ?? 0);

            var contextCopy = ctx.Clone();
            contextCopy.Y = Bounds.Y + Padding.Top;
            contextCopy.X = Bounds.X + Padding.Left;

            foreach (var c in Children)
            {
                contextCopy.MaxW = Bounds.Width - Padding.Right - Padding.Left;
                contextCopy.Width = null;
                var sizeChild = c.CalculateSize(contextCopy);
                contextCopy.Height = sizeChild.Height;
                contextCopy.Width = sizeChild.Width;
                c.Arrange(contextCopy);
                contextCopy.Y += sizeChild.Height;
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