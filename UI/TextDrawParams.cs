using System.Diagnostics;
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
        public NVGalign AlignFlags;

        public void Draw(NVGcontext vg)
        {
            vg.FontFace(Font);
            vg.FontSize(Size);
            vg.FillColor(Color);          
            vg.TextAlign((int)NVGalign.NVG_ALIGN_TOP | (int)AlignFlags);
            vg.TextBox(Rect.X, Rect.Y, Rect.Width, Text);
        }


    }
}
