using System.Numerics;
using XPlat.NanoVg;

namespace XPlat.Gui
{
    public class Label : Widget
    {
        

        public Label(Widget? parent = null) : base(parent)
        {
        }

        public string Text { get; set; }
        public Icon Icon { get; set; } = Icon.NONE;

        public override Vector2 PreferredSize(NVGcontext vg)
        {
            if (string.IsNullOrEmpty(Text) && Icon == Icon.NONE) return Vector2.Zero;

            if(Icon != Icon.NONE){
                vg.FontFace("icons");
                vg.FontSize(18);
            } else {
                vg.FontFace("sans");
                vg.FontSize(18);
            }

            var bounds = new float[4];
            if (FixedSize != Vector2.Zero)
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_TOP);
                vg.TextBoxBounds(Position.X, Position.Y, FixedSize.X, DrawText, bounds);
                return new Vector2(FixedSize.X, bounds[3] - bounds[1]);
            }
            else
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);
                vg.TextBounds(0, 0, DrawText, bounds);
                return new Vector2(bounds[2] - bounds[0] + 2, bounds[3] - bounds[1]);
            }
        }

        private string DrawText => Icon != Icon.NONE ? char.ConvertFromUtf32((int)Icon) : Text;

        public override void Draw(NVGcontext vg)
        {
            base.Draw(vg);
            
            if(Icon != Icon.NONE){
                vg.FontFace("icons");
                vg.FontSize(18);
            } else {
                vg.FontFace("sans");
                vg.FontSize(18);
            }

            vg.FillColor(MouseFocus ? "#fff" : "#000");
            if (FixedSize != Vector2.Zero)
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_TOP);
                vg.TextBox(Position.X, Position.Y, FixedSize.X, DrawText);
            }
            else
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);
                vg.Text(Position.X, Position.Y + Size.Y * 0.5f, DrawText);
            }
        }


    }
}