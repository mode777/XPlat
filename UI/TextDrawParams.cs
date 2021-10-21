using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public struct TextDrawParams
    {
        public string Text;
        public float Size;
        public string Font;
        public RectangleF Rect;
        public NVGcolor Color;

        public void Draw(NVGcontext vg)
        {
            vg.FontFace(Font);
            vg.FontSize(Size);
            vg.FillColor(Color);          
            vg.TextBox(Rect.X, Rect.Y, Rect.Width, Text);
            DrawDebugRect(vg);
        }

        public void DrawDebugRect(NVGcontext vg)
        {
            vg.BeginPath();
            vg.Rect(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            vg.StrokeColor("#ff00ff");
            vg.Stroke();
        }
    }
}
