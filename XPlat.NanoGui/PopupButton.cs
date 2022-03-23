using System.Numerics;
using XPlat.NanoVg;

namespace XPlat.NanoGui
{
    public class PopupButton : Button
    {
        public int ChevronIcon { get; set; }
        public Popup Popup { get; set; }
        
        public PopupButton(Widget? parent, string caption, int icon) 
            : base(parent, caption, icon)
        {
            ChevronIcon = Theme.PopupChevronRightIcon;

            Flags = ButtonFlags.ToggleButton | ButtonFlags.PopupButton;

            Popup = new Popup(Screen, Window);
            Popup.Size = new Vector2(320, 250);
            Popup.Visible = false;

            IconExtraScale = 0.8f;
        }

        public override Vector2 PreferredSize(NVGcontext vg)
        {
            return base.PreferredSize(vg) + new Vector2(15, 0);
        }

        public override void Draw(NVGcontext vg)
        {
            if(!Enabled && Pushed) Pushed = false;

            Popup.Visible = Pushed;
            base.Draw(vg);

            if(ChevronIcon != 0){
                var icon = char.ConvertFromUtf32(ChevronIcon);
                var textColor = TextColor.a == 0 
                    ? Theme.TextColor
                    : TextColor;

                vg.FontSize((FontSize < 0 ? Theme.ButtonFontSize : FontSize) * IconScale);
                vg.FontFace("icons");
                vg.FillColor(Enabled ? TextColor : Theme.DisabledTextColor);
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);

                float iw = vg.TextBounds(0,0,icon, null);
                var iconPos = new Vector2(0, Position.Y + Size.Y * 0.5f - 1);

                if(Popup.Side == PopupSide.Right) iconPos.X = Position.X + Size.X - iw - 8;
                else iconPos.X = Position.X + 8;

                vg.Text(iconPos.X, iconPos.Y, icon);
            }
        }

        public override void PerformLayout(NVGcontext ctx)
        {
            base.PerformLayout(ctx);

            var parentWindow = Window;

            var anchorSize = Popup.AnchorSize;

            if(parentWindow != null){
                var posY = AbsolutePosition.Y - parentWindow.Position.Y + Size.Y / 2;
                if(Popup.Side == PopupSide.Right)
                    Popup.AnchorPos = new Vector2(parentWindow.Width + anchorSize, posY);
                else 
                    Popup.AnchorPos = new Vector2(-anchorSize, posY);
            } else {
                Popup.Position = AbsolutePosition + new Vector2(Width + anchorSize + 1, Size.Y / 2 - anchorSize);
            }
        }

        public void SetSide(PopupSide side){
            if(Popup.Side == PopupSide.Right && ChevronIcon == Theme.PopupChevronRightIcon)
                ChevronIcon = Theme.PopupChevronLeftIcon;
            else if(Popup.Side == PopupSide.Left && ChevronIcon == Theme.PopupChevronLeftIcon)
                ChevronIcon = Theme.PopupChevronRightIcon;
            
            Popup.Side = side;
        }
    }
}