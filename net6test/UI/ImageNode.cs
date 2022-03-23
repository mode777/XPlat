using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class ImageNode : Element
    {
        public ImageNode()
        {
            Style = new Style(Style.Default);
            HoverStyle = new Style(Style);
        }

        public Style Style { get; set; }
        public Style HoverStyle { get; }
        public NvgImage Image { get; set; }
        private ImageDrawParams drawParams;

        public override void Update(NVGcontext ctx)
        {            
            base.Update(ctx);
            var style = HasMouseOver ? HoverStyle : Style;
            drawParams = new ImageDrawParams(ctx, Image, Bounds, Style.FillStrategy ?? ImageFillStrategy.Stretch);
        }

        public override void Arrange(UiContext ctx)
        {
            SetPixelPos(ctx.X ?? 0, ctx.Y ?? 0);
            SetPixelSize(ctx.MaxW ?? Image.Width, ctx.MaxH ?? Image.Height);
        }

        public override void Draw(NVGcontext vg)
        {
            drawParams.Draw(vg);
            base.Draw(vg);
        }



    }
}