using System.Numerics;
using NanoVGDotNet;
using SDL2;

namespace net6test.NanoGuiPort
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

    public class Button : Widget
    {
        public Button(Widget? parent, string caption, int icon) : base(parent)
        {
            Caption = caption;
            Icon = icon;
            Flags = ButtonFlags.NormalButton;
            ButtonGroup = new List<Button>();
        }

        public string Caption { get; }
        public int Icon { get; }
        public ButtonFlags Flags { get; set; }
        public bool Pushed { get; set; }
        public List<Button> ButtonGroup { get; }
        public event EventHandler<bool> OnChange;

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
                if (NanoVG.nvgIsFontIcon(Icon))
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
                    } else if(Flags.HasFlag(ButtonFlags.PopupButton))
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
                }

            }
        }

        public override void Draw(NVGcontext vg)
        {
            base.Draw(vg);
        }
    }
}