using System.Numerics;
using NanoVGDotNet;

namespace net6test.NanoGuiPort
{
    public class Popup : Window
    {
        public Popup(Widget parent, Window window) : base(parent, "")
        {
            ParentWindow = window;
            AnchorOffset = 30;
            AnchorSize = 15;
            Side = PopupSide.Right;
        }

        public Vector2 AnchorPos;
        public Window ParentWindow { get; set; }
        public float AnchorOffset { get; set; }
        public float AnchorSize { get; set; }
        public PopupSide Side { get; set; }

        public override void PerformLayout(NVGcontext ctx)
        {
            if(Layout != null || Children.Count != 1){
                base.PerformLayout(ctx);
            } else {
                Children[0].Position = Vector2.Zero;
                Children[0].Size = Size;
                Children[0].PerformLayout(ctx);
            }
            if(Side == PopupSide.Left) AnchorPos.X -= Size.X;
        }

        public override void RefreshRelativePlacement(){
            if(ParentWindow == null) return;
            ParentWindow.RefreshRelativePlacement();
            Visible = ParentWindow.VisibleRecursive() ? Visible : false;
            Position = ParentWindow.Position + AnchorPos - new Vector2(0, AnchorOffset);
        }

        public override void Draw(NVGcontext vg)
        {
            RefreshRelativePlacement();

            if(!Visible) return;

            var ds = Theme.WindowDropShadowSize;
            var cr = Theme.WindowCornerRadius;

            vg.Save();
            vg.ResetScissor();

            var shadowPaint = vg.BoxGradient(Position.X, Position.Y, 
                Size.X, Size.Y, cr*2, ds*2, Theme.DropShadow, Theme.Transparent);

            vg.BeginPath();
            vg.Rect(Position.X-ds, Position.Y-ds, Size.X+2*ds, Size.Y+2*ds);
            vg.RoundedRect(Position.X, Position.Y, Size.X, Size.Y, cr);
            vg.PathWinding((int)NVGsolidity.NVG_HOLE);
            vg.FillPaint(shadowPaint);
            vg.Fill();

            vg.BeginPath();
            vg.RoundedRect(Position.X, Position.Y, Size.X, Size.Y, cr);

            var @base = Position + new Vector2(0, AnchorOffset);
            int sign = -1;
            if(Side == PopupSide.Left){
                @base.X += Size.X;
                sign = 1;
            }

            vg.MoveTo(@base.X + AnchorSize * sign, @base.Y);
            vg.LineTo(@base.X - 1 * sign, @base.Y - AnchorSize);
            vg.LineTo(@base.X - 1 * sign, @base.Y + AnchorSize);

            vg.FillColor(Theme.WindowPopup);
            vg.Fill();
            vg.Restore();

            base.DrawChildren(vg);
        }
    }

    public enum PopupSide
    {
        Left,
        Right
    }
}