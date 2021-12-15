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
            Screen?.CenterWindow(this);
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

        protected virtual void RefreshRelativePlacement()
        {
            /* Overridden in \ref Popup */
        }
    }
}