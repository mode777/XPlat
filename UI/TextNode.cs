using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class TextNode : Element
    {
        public NVGcolor Color { get; set; } = NanoVG.nvgRGBA(0, 0, 0, 255);
        public string Text { get; set; }
        public Quantity Size { get; set; } = "4vh";
        public string Font { get; set; } = "sans";
        public NVGalign TextAlign { get; set; } = NVGalign.NVG_ALIGN_LEFT;
        private TextDrawParams drawParams;
        private float[] bounds = new float[4];

        public override void Update(NVGcontext ctx)
        {            
            base.Update(ctx);
            drawParams = new TextDrawParams
            {
                Color = Color,
                Text = Text,
                Size = Size,
                Font = Font,
                Rect = Bounds,
                AlignFlags = TextAlign
            };
        }
        public override SizeF CalculateSize(UiContext ctx)
        {            
            ctx.Vg.FontFace(Font);
            ctx.Vg.FontSize(Size);
            var width = ctx.MaxX.HasValue ? ctx.MaxX.Value : float.MaxValue;
            ctx.Vg.TextBoxBounds(Bounds.X, Bounds.Y, width, Text, bounds);
            return new SizeF(width, bounds[3] - bounds[1]);
        }

        public override void Arrange(UiContext ctx)
        {
        }

        public override void Draw(NVGcontext vg)
        {
            drawParams.Draw(vg);
            base.Draw(vg);
        }



    }
}