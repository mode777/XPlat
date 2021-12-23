using System.Numerics;
using NanoVGDotNet;
using SDL2;

namespace net6test.NanoGuiPort
{

    public class Window : Widget, IDisposable
    {
        protected bool drag;
        private Widget buttonPanel;

        public Window(Widget parent, string title = "Untitled") : base(parent)
        {
            Title = title;
            drag = false;
        }

        public bool Modal { get; set; }
        public string Title { get; set; }

        public Widget ButtonPanel { 
            get { 
                if(buttonPanel == null){
                    buttonPanel = new Widget(this);
                    buttonPanel.Layout = new BoxLayout(Orientation.Horizontal, Alignment.Middle, 0, 4);
                }
                return buttonPanel; 
            } 
            set => buttonPanel = value; 
        }

        public void Dispose()
        {
            Screen?.DisposeWindow(this);
        }

        public void Center()
        {
            var screen = Screen;
            screen?.CenterWindow(this);
        }

        public override void Draw(NVGcontext vg)
        {
            float ds = Theme.WindowDropShadowSize;
            float cr = Theme.WindowCornerRadius;
            float hh = Theme.WindowHeaderHeight;

            vg.Save();
            vg.BeginPath();
            vg.RoundedRect(Position.X, Position.Y, Size.X, Size.Y, cr);

            vg.FillColor(mouseFocus ? Theme.WindowFillFocused : Theme.WindowFillUnfocused);
            
            vg.Fill();

            var shadowPaint = vg.BoxGradient(Position.X, Position.Y, Size.X, Size.Y, cr * 2, ds * 2, Theme.DropShadow, Theme.Transparent);

            vg.Save();
            vg.ResetScissor();
            vg.BeginPath();
            vg.Rect(Position.X - ds, Position.Y - ds, Size.X + 2 * ds, Size.Y + 2 * ds);
            vg.RoundedRect(Position.X, Position.Y, Size.X, Size.Y, cr);
            vg.PathWinding((int)NVGsolidity.NVG_HOLE);
            vg.FillPaint(shadowPaint);
            vg.Fill();
            vg.Restore();

            if (!String.IsNullOrEmpty(Title))
            {
                var headerPaint = vg.LinearGradient(Position.X, Position.Y, Position.X, Position.Y + hh, Theme.WindowHeaderGradientTop, Theme.WindowHeaderGradientBottom);

                vg.BeginPath();
                vg.RoundedRect(Position.X, Position.Y, Size.X, hh, cr);

                vg.FillPaint(headerPaint);
                vg.Fill();

                vg.BeginPath();
                vg.RoundedRect(Position.Y, Size.X, Size.X, hh, cr);
                vg.StrokeColor(Theme.WindowHeaderSepTop);

                vg.Save();
                vg.IntersectScissor(Position.X, Position.Y, Size.X, 0.5f);
                vg.Stroke();
                vg.Restore();

                vg.BeginPath();
                vg.MoveTo(Position.X + 0.5f, Position.Y + hh - 1.5f);
                vg.LineTo(Position.X + Size.X - 0.5f, Position.Y + hh - 1.5f);
                vg.StrokeColor(Theme.WindowHeaderSepBot);
                vg.Stroke();

                vg.FontSize(18);
                vg.FontFace("sans-bold");
                vg.TextAlign((int)NVGalign.NVG_ALIGN_CENTER | (int)NVGalign.NVG_ALIGN_MIDDLE);

                vg.FontBlur(2);
                vg.FillColor(Theme.DropShadow);
                vg.Text(Position.X + Size.X / 2, Position.Y + hh / 2, Title);
                vg.FontBlur(0);
                vg.FillColor(Focused ? Theme.WindowTitleFocused : Theme.WindowTitleUnfocused);
                vg.Text(Position.X + Size.X / 2, Position.Y + hh / 2 - 1, Title);

                vg.Restore();
            }

            vg.Restore();

            base.Draw(vg);
        }

        public override bool MouseEnterEvent(Vector2 p, bool enter)
        {
            base.MouseEnterEvent(p, enter);
            return true;
        }

        public override bool MouseDragEvent(Vector2 p, Vector2 rel, int button, int modifiers)
        {
            if(drag && button == SDL.SDL_BUTTON_LEFT){
                Position += rel;
                Position = Vector2Ext.Max(Position, Vector2.Zero);
                Position = Vector2Ext.Min(Position, Parent.Size - Size);
                return true;
            }
            return false;
        }

        public override bool MouseButtonEvent(Vector2 p, int button, bool down, int modifiers)
        {
            if(base.MouseButtonEvent(p, button, down, modifiers)) return true;
            if(button == SDL.SDL_BUTTON_LEFT){
                drag = down && (p.Y - Position.Y) < Theme?.WindowHeaderHeight;
                return true;
            }
            return false;
        }

        public override bool ScrollEvent(Vector2 p, Vector2 rel)
        {
            base.ScrollEvent(p, rel);
            return true;
        }

        public override Vector2 PreferredSize(NVGcontext vg)
        {
            if (buttonPanel != null) ButtonPanel.Visible = false;
            var result = base.PreferredSize(vg);
            if (buttonPanel != null) ButtonPanel.Visible = true;

            vg.FontFace("sans-bold");
            vg.FontSize(18);
            var bounds = new float[4];
            vg.TextBounds(0, 0, Title, bounds);

            return new Vector2(MathF.Max(result.X, bounds[2] - bounds[0] + 20), MathF.Max(result.Y, bounds[3] - bounds[1]));
        }

        public override void PerformLayout(NVGcontext ctx)
        {
            if(buttonPanel == null){
                base.PerformLayout(ctx);
            } else {
                buttonPanel.Visible = false;
                base.PerformLayout(ctx);
                foreach (var w in buttonPanel.Children)
                {
                    w.FixedSize = new Vector2(22,22);
                    w.FontSize = 15;
                }
                buttonPanel.Visible = true;
                buttonPanel.Size = new Vector2(Width, 22);
                buttonPanel.Position = new Vector2(Width - buttonPanel.PreferredSize(ctx).X + 5, 3);
                buttonPanel.PerformLayout(ctx);
            }
        }

        public virtual void RefreshRelativePlacement()
        {
            /* Overridden in \ref Popup */
        }
    }
}