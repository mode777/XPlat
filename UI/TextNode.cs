using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class TextNode : Element
    {
        public TextNode()
        {
            Style = new Style(Style.Default);
            HoverStyle = new Style(Style);
        }

        public Style Style { get; set; }
        public Style HoverStyle { get; }
        public string Text { get; set; }
        public Quantity Size { get; set; } = "4vh";
        public string Font { get; set; } = "sans";
        public NVGalign TextAlign { get; set; } = NVGalign.NVG_ALIGN_LEFT;
        private TextDrawParams drawParams;
        private float[] bounds = new float[4];

        public override void Update(NVGcontext ctx)
        {            
            base.Update(ctx);
            var style = HasMouseOver ? HoverStyle : Style;
            drawParams = new TextDrawParams
            {
                Color = style.FontColor.Value,
                Text = Text,
                Size = Size,
                Font = Font,
                Rect = Bounds,
                AlignFlags = TextAlign
            };
        }
        // public override SizeF CalculateSize(UiContext ctx)
        // {            
        //     ctx.Vg.FontFace(Font);
        //     ctx.Vg.FontSize(Size);
        //     var width = ctx.Width ?? ctx.MaxW ?? 0;
        //     ctx.Vg.TextBoxBounds(ctx.X ?? 0,ctx.Y ?? 0, width, Text, bounds);
        //     return new SizeF(width, bounds[3] - bounds[1]);
        // }

        public override void Arrange(UiContext ctx)
        {
            SetPixelPos(ctx.X ?? 0, ctx.Y ?? 0);

            ctx.Vg.FontFace(Font);
            ctx.Vg.FontSize(Size);
            var width = ctx.MaxW ?? 0;
            ctx.Vg.TextBoxBounds(ctx.X ?? 0,ctx.Y ?? 0, width, Text, bounds);
            var height = bounds[3] - bounds[1];

            SetPixelSize(width, height);
        }

        public override void Draw(NVGcontext vg)
        {
            drawParams.Draw(vg);
            base.Draw(vg);
        }



    }
}