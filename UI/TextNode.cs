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

        public void Update(NVGcontext ctx)
        {
            drawParams = new TextDrawParams
            {
                Color = Color,
                Text = Text,
                Size = Size,
                Font = Font,
                //Rect = ???
            };
        }
        
        protected override RectangleF CalculateBounds(NVGcontext vg)
        {
            throw new NotImplementedException();
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

        public void Draw(NVGcontext vg)
        {

            //vg.FontFace(Font);
            //vg.FontSize((int)SizeScaled);
            //vg.FillColor(Color);
            //float asc = -1, desc = -1, lh = -1;
            //vg.TextMetrics(ref asc, ref desc, ref lh);
            //vg.TextBox(bounds.X, bounds.Y+lh, bounds.Width, Text);
            //DrawDebugRect(vg);
        }


        //public void DrawDebugRect(NVGcontext vg)
        //{
        //    vg.BeginPath();
        //    vg.Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        //    vg.StrokeColor("#ff00ff");
        //    vg.Stroke();
        //}
    }
}