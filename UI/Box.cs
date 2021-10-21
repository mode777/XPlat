using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{

    public class Box : Node
    {
        private BoxDrawParams _drawParams;

        public FillStyle? Fill { get; set; }
        public FillStyle? HoverFill { get; set; }
        public Quantity CornerRadius { get; set; }
        public Shadow? Shadow { get; set; }
        public Thickness Padding { get; set; }
        public RectQ Rect { get; set; }

        public override void Update(UiContext ctx)
        {
            base.Update(ctx);
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

        protected override RectangleF CalculateBounds(UiContext ctx)
        {

        }

        protected override void Arrange(UiContext ctx){
            var bounds = base.CalculateBounds(ctx);
            var copy = ctx.Clone();

        }

        public void Draw(NVGcontext vg)
        {
            _drawParams.Draw(vg);
        }

        // protected override SizeF CalculateSize(UiContext ctx) => new SizeF(Rect.Width, Rect.Height);

        // protected override PointF CalculatePos(UiContext ctx) => new PointF(Rect.X, Rect.Y);

    }
}