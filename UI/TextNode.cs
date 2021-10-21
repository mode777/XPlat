using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class TextNode : Node
    {
        public NVGcolor Color { get; set; } = NanoVG.nvgRGBA(0, 0, 0, 255);
        public string Text { get; set; }
        public Quantity Size { get; set; } = "4vh";
        public string Font { get; set; } = "sans";
                
        private TextDrawParams drawParams;
        private float[] bounds = new float[4];
        private float lineHeight;

        public override void Update(NVGcontext ctx)
        {            

            drawParams = new TextDrawParams
            {
                Color = Color,
                Text = Text,
                Size = Size,
                Font = Font,
                LineHeight = lineHeight,
                Rect = Bounds,
            };
        }
        public override SizeF CalculateSize(UiContext ctx)
        {            
            ctx.Vg.FontFace(Font);
            ctx.Vg.FontSize(Size);
            float _ = 0;
            ctx.Vg.TextMetrics(ref _, ref _, ref lineHeight);
            ctx.Vg.TextBoxBounds(Bounds.X, Bounds.Y, ctx.MaxX.HasValue ? ctx.MaxX.Value : float.MaxValue, Text, bounds);
            return new SizeF(bounds[2] - bounds[0], bounds[3] - bounds[1]);
        }

        public override void Arrange(UiContext ctx)
        {
        }



        // public bool FitInto(RectangleF requested, NVGcontext vg, IPlatformInfo p)
        // {
        //     SizeScaled = p.Size(Size);
        //     bounds = requested;

        //     vg.FontFace(Font);
        //     vg.FontSize((int)SizeScaled);

        //     var res = new float[4];

        //     vg.TextBoxBounds(requested.X, requested.Y, requested.Width, Text, res);

        //     bounds.Height = res[3] - res[1];

        //     return bounds.Height > requested.Height;
        // }

        public override void Draw(NVGcontext vg)
        {
            drawParams.Draw(vg);
        }

    }
}