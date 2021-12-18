using System.Numerics;
using NanoVGDotNet;

namespace net6test.NanoGuiPort
{
    public class Label : Widget
    {
        public NVGcolor Color { get; set; }
        public string Font { get; set; }
        

        public Label(Widget? parent, string caption, string font = "sans-bold", float fontSize = 0) : base(parent)
        {
            Caption = caption;
            Font = font;
            if (Theme != null)
            {
                FontSize = Theme.StandardFontSize;
                Color = Theme.TextColor;
            }
            if (fontSize > 0) FontSize = fontSize;
        }

        public override Theme? Theme
        {
            get => base.Theme;
            set
            {
                base.Theme = value;
                if(value != null)
                {
                    FontSize = Theme.StandardFontSize;
                    Color = Theme.TextColor;
                }
            }
        }

        public string Caption { get; set; }

        public override Vector2 PreferredSize(NVGcontext vg)
        {
            if (string.IsNullOrEmpty(Caption)) return Vector2.Zero;

            vg.FontFace(Font);
            vg.FontSize(FontSize);
            var bounds = new float[4];
            if(FixedSize != Vector2.Zero)
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_TOP);
                vg.TextBoxBounds(Position.X, Position.Y, FixedSize.X, Caption, bounds);
                return new Vector2(FixedSize.X, bounds[3] - bounds[1]);
            } else
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);
                vg.TextBounds(0, 0, Caption, bounds);
                return new Vector2(bounds[2]-bounds[0] + 2, bounds[3] - bounds[1]);
            }
        }

        public override void Draw(NVGcontext vg)
        {
            base.Draw(vg);
            vg.FontFace(Font);
            vg.FontSize(FontSize);
            vg.FillColor(Color);
            if(FixedSize != Vector2.Zero)
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_TOP);
                vg.TextBox(Position.X, Position.Y, FixedSize.X, Caption);
            } else
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);
                vg.Text(Position.X, Position.Y + Size.Y * 0.5f, Caption);
            }
        }
    }
}