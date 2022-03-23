using System;
using System.Collections.Generic;
using System.Numerics;
using SDL2;
using XPlat.NanoVg;

namespace XPlat.NanoGui
{


    [Flags]
    public enum ButtonFlags
    {
        NormalButton = 1 << 0,
        RadioButton = 1 << 1,
        ToggleButton = 1 << 2,
        PopupButton = 1 << 3,
        MenuButton = 1 << 4,
    }

    public enum IconPosition {
        Left,
        LeftCentered,
        Right,
        RightCentered
    }

    public class Button : Widget
    {
        public Button(Widget? parent, string caption, int icon = 0) : base(parent)
        {
            Caption = caption;
            Icon = icon;
            Flags = ButtonFlags.NormalButton;
            ButtonGroup = new List<Button>();
            IconPosition = IconPosition.LeftCentered;
        }

        public string Caption { get; set; }
        public int Icon { get; set; }
        public ButtonFlags Flags { get; set; }
        public bool Pushed { get; set; }
        public List<Button> ButtonGroup { get; }
        public NVGcolor BackgroundColor { get; set; }
        public NVGcolor TextColor { get; set; }
        public IconPosition IconPosition { get; set; }
        public event EventHandler<bool> OnChange;
        public event EventHandler OnPush;

        public override Vector2 PreferredSize(NVGcontext vg)
        {
            float fontSize = FontSize == -1 ? Theme.ButtonFontSize : FontSize;

            vg.FontSize(fontSize);
            vg.FontFace("sans-bold");
            float tw = vg.TextBounds(0,0, Caption, null);
            float iw = 0;
            float ih = fontSize;

            if(Icon != 0)
            {
                if (NanoVgApi.nvgIsFontIcon(Icon))
                {
                    ih *= IconScale;
                    vg.FontFace("icons");
                    vg.FontSize(ih);
                    iw = vg.TextBounds(0, 0, char.ConvertFromUtf32(Icon), null) + Size.Y * 0.15f;
                } else
                {
                    int w = 0, h = 0;
                    ih *= 0.9f;
                    vg.ImageSize(Icon, ref w, ref h);
                    iw = w * ih / h;
                }
            }
            return new Vector2((tw+iw)+20, fontSize + 10);
        }

        public override bool MouseEnterEvent(Vector2 p, bool enter)
        {
            base.MouseEnterEvent(p, enter);
            return true;
        }

        public override bool MouseButtonEvent(Vector2 p, int button, bool down, int modifiers)
        {
            base.MouseButtonEvent(p, button, down, modifiers);

            if(Enabled && 
                (button == SDL.SDL_BUTTON_LEFT && !Flags.HasFlag(ButtonFlags.MenuButton)) ||
                (button == SDL.SDL_BUTTON_RIGHT && Flags.HasFlag(ButtonFlags.MenuButton)))
            {
                bool pushedBackup = Pushed;
                if (down)
                {
                    if (Flags.HasFlag(ButtonFlags.RadioButton))
                    {
                        if(ButtonGroup.Count == 0)
                        {
                            foreach (var widget in Parent.Children)
                            {
                                if(widget is Button b && b != this && b.Pushed)
                                {
                                    b.Pushed = false;
                                    b.OnChange?.Invoke(b, false);
                                }
                            }
                        } else
                        {
                            foreach (var b in ButtonGroup)
                            {
                                if(b != this && b.Flags.HasFlag(ButtonFlags.RadioButton) && b.Pushed)
                                {
                                    b.Pushed = false;
                                    b.OnChange?.Invoke(b, false);
                                }
                            }
                        }
                    } 
                    if(Flags.HasFlag(ButtonFlags.PopupButton))
                    {
                        foreach (var widget in Parent.Children)
                        {
                            if(widget is Button b && b.Flags.HasFlag(ButtonFlags.PopupButton) && b.Pushed)
                            {
                                b.Pushed = false;
                                b.OnChange?.Invoke(b, false);
                            }
                        }                        
                        (this as PopupButton)?.Popup.RequestFocus();
                    }
                    if(Flags.HasFlag(ButtonFlags.ToggleButton)){
                        Pushed = !Pushed;
                    } else {
                        Pushed = true;
                    }
                } else if(Pushed || Flags.HasFlag(ButtonFlags.MenuButton)) {
                    if(Contains(p)) OnPush?.Invoke(this, EventArgs.Empty);
                    if(Flags.HasFlag(ButtonFlags.NormalButton)) Pushed = false;
                }
                if(pushedBackup != Pushed) OnChange?.Invoke(this, Pushed);

                return true;

            }
            return false;
        }

        public override void Draw(NVGcontext vg)
        {
            base.Draw(vg);

            var gradTop = Theme.ButtonGradientTopUnfocused;
            var gradBot = Theme.ButtonGradientBotUnfocused;

            if(Pushed || (mouseFocus && (Flags.HasFlag(ButtonFlags.MenuButton)))){
                gradTop = Theme.ButtonGradientTopPushed;
                gradBot = Theme.ButtonGradientBotPushed;
            } else if(mouseFocus && Enabled){
                gradTop = Theme.ButtonGradientTopFocused;
                gradBot = Theme.ButtonGradientBotFocused;
            }

            vg.BeginPath();

            vg.RoundedRect(Position.X + 1, Position.Y + 1, Size.X - 2, Size.Y - 2, Theme.ButtonCornerRadius);

            if(BackgroundColor.a != 0){
                var c = BackgroundColor;
                c.a = 1;
                vg.FillColor(c);
                vg.Fill();
                if(Pushed){
                    gradTop.a = gradBot.a = 0.8f;
                } else {
                    var v = 1f - BackgroundColor.a;
                    gradTop.a = gradBot.a = Enabled ? v : v * .5f + .5f;
                }
            }

            var bg = vg.LinearGradient(Position.X, Position.Y, Position.X, Position.Y + Size.Y, gradTop, gradBot);

            vg.FillPaint(bg);
            vg.Fill();

            vg.BeginPath();
            vg.StrokeWidth(1);
            vg.RoundedRect(Position.X + 0.5f, Position.Y + (Pushed ? 0.5f : 1.5f), Size.X - 1,
                Size.Y - 1 - (Pushed ? 0 : 1), Theme.ButtonCornerRadius);
            vg.StrokeColor(Theme.BorderLight);
            vg.Stroke();

            vg.BeginPath();
            vg.RoundedRect(Position.X +0.5f, Position.Y + 0.5f, Size.X -1, Size.Y - 2, Theme.ButtonCornerRadius);
            vg.StrokeColor(Theme.BorderDark);
            vg.Stroke();

            var fontSize = FontSize == -1 ? Theme.ButtonFontSize : FontSize;
            vg.FontSize(fontSize);
            vg.FontFace("sans-bold");
            float tw = vg.TextBounds(0,0,Caption, null);

            var center = Position + (Size * 0.5f);
            var textPos = new Vector2(center.X - tw * 0.5f, center.Y -1);
            var textColor = TextColor.a == 0 ? Theme.TextColor : TextColor;
            if(!Enabled) textColor = Theme.DisabledTextColor;

            if(Icon != 0){
                var icon = char.ConvertFromUtf32(Icon);

                float iw;
                var ih = fontSize;
                if(NanoVgApi.nvgIsFontIcon(Icon)){
                    ih *= IconScale;
                    vg.FontSize(ih);
                    vg.FontFace("icons");
                    iw = vg.TextBounds(0,0,icon, null);
                } else {
                    int w=0, h=0;
                    ih *= 0.9f;
                    vg.ImageSize(Icon, ref w, ref h);
                    iw = w * ih / h;
                }
                if(!string.IsNullOrEmpty(Caption)) iw += Size.Y * 0.15f;
                vg.FillColor(textColor);
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);
                var iconPos = center;
                iconPos.Y -= 1;

                if(IconPosition == IconPosition.LeftCentered){
                    iconPos.X -= (tw + iw) * 0.5f;
                    textPos.X += iw * 0.5f;
                } else if(IconPosition == IconPosition.RightCentered){
                    textPos.X -= iw * 0.5f;
                    iconPos.X += tw * 0.5f;
                } else if(IconPosition == IconPosition.Left) {
                    iconPos.X -= Position.X + 8;
                } else if(IconPosition == IconPosition.Right) {
                    iconPos.X -= Position.X + Size.X - iw - 8;
                }

                if(NanoVgApi.nvgIsFontIcon(Icon)){
                    vg.Text(iconPos.X, iconPos.Y+1, icon);
                } else {
                    var imgPaint = vg.ImagePattern(iconPos.X, iconPos.Y - ih/2, iw, ih, 0, Icon, Enabled ? 0.5f : 0.25f);
                    vg.FillPaint(imgPaint);
                    vg.Fill();
                }

            }

            vg.FontSize(fontSize);
            vg.FontFace("sans-bold");
            vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);
            vg.FillColor(Theme.TextColorShadow);
            vg.Text(textPos.X, textPos.Y, Caption);
            vg.FillColor(textColor);
            vg.Text(textPos.X, textPos.Y + 1, Caption);

        }
    }
}