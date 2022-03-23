using System;
using System.Numerics;
using XPlat.NanoVg;

namespace XPlat.NanoGui
{
    public class Checkbox : Widget
    {
        event EventHandler<bool> OnChange;
        public string Caption { get; set; }
        public bool Pushed { get; set; }
        public bool Checked { get; set; }

        public Checkbox(Widget? parent, string caption) : base(parent)
        {
            IconExtraScale = 1.2f;
            Caption = caption;
        }

        public override bool MouseButtonEvent(Vector2 p, int button, bool down, int modifiers)
        {
            base.MouseButtonEvent(p, button, down, modifiers);
            if(!Enabled) return false;

            if(button == SDL2.SDL.SDL_BUTTON_LEFT){
                if(down) Pushed = true;
                else if(Pushed) {
                    if(Contains(p)){
                        Checked = !Checked;
                        OnChange?.Invoke(this, Checked);
                    }
                    Pushed = false;
                }
                return true;
            }
            return false;
        }

        public override Vector2 PreferredSize(NVGcontext vg)
        {
            if(FixedSize != Vector2.Zero) return FixedSize;
            vg.FontSize(FontSize);
            vg.FontFace("sans");
            return new Vector2(
                vg.TextBounds(0,0,Caption, null) + 1.8f * FontSize,
                FontSize * 1.3f);
        }

        public override void Draw(NVGcontext vg)
        {
            DrawChildren(vg);

            vg.FontSize(FontSize);
            vg.FontFace("sans");
            vg.FillColor(Enabled ? Theme.TextColor : Theme.DisabledTextColor);
            vg.TextAlign((int) NVGalign.NVG_ALIGN_LEFT | (int) NVGalign.NVG_ALIGN_MIDDLE);
            vg.Text(Position.X + 1.6f * FontSize, Position.Y + Size.Y * 0.5f, Caption);
            var bg = vg.BoxGradient(Position.X + 1.5f, Position.Y + 1.5f, Size.Y - 2, Size.Y - 2, 3, 3, 
                Pushed ? vg.RGBA(0,0,0,100) : vg.RGBA(0,0,0,32), vg.RGBA(0,0,0,180));

            vg.BeginPath();
            vg.RoundedRect(Position.X + 1, Position.Y + 1, Size.Y - 2, Size.Y - 2, 3);
            vg.FillPaint(bg);
            vg.Fill();

            if(Checked){
                vg.FontSize(IconScale * Size.Y);
                vg.FontFace("icons");
                vg.FillColor(Enabled ? Theme.IconColor : Theme.DisabledTextColor);
                vg.TextAlign((int)NVGalign.NVG_ALIGN_CENTER | (int)NVGalign.NVG_ALIGN_MIDDLE);
                vg.Text(Position.X + Size.Y * 0.5f + 1, Position.Y + Size.Y * 0.5f, char.ConvertFromUtf32(Theme.CheckBoxIcon));
            }
        }
    }
}