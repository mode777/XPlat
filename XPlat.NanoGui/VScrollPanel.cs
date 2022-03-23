using System;
using System.Linq;
using System.Numerics;
using XPlat.NanoVg;


namespace XPlat.NanoGui
{
    public class VScrollPanel : Widget
    {
        public VScrollPanel(Widget? parent) : base(parent)
        {
            this.ChildPreferredHeight = 0f;
            this.Scroll = 0f; 
            this.UpdateLayout = false;
        }

        public float ChildPreferredHeight { get; private set; }
        public float Scroll { get; set; }
        public bool UpdateLayout { get; set; }

        public override void PerformLayout(NVGcontext ctx)
        {
            base.PerformLayout(ctx);

            if(Children.Count == 0) return;
            if(Children.Count > 1) throw new Exception("VScrollPanel should have one child");

            var child = Children.First();
            ChildPreferredHeight = child.PreferredSize(ctx).Y;

            if(ChildPreferredHeight > Size.Y){
                child.Position = new Vector2(0, -Scroll * (ChildPreferredHeight - Size.Y));
                Size = new Vector2(Size.X - 12, ChildPreferredHeight);
            } else {
                child.Position = Vector2.Zero;
                child.Size = Size;
                Scroll = 0;
            }
            child.PerformLayout(ctx);
        }

        public override Vector2 PreferredSize(NVGcontext ctx)
        {
            if(Children.Count == 0) return Vector2.Zero;
            return Children.First().PreferredSize(ctx) + new Vector2(12, 0);
        }

        public override bool MouseDragEvent(Vector2 p, Vector2 rel, int button, int modifiers)
        {
            if(Children.Count > 0 && ChildPreferredHeight > Size.Y){
                var scrollh = Height * MathF.Min(1f, Height / ChildPreferredHeight);

                Scroll = MathF.Max(0f, MathF.Min(1, Scroll + rel.Y / (Size.Y - 8 - scrollh)));
                UpdateLayout = true;
                return true;
            } else {            
                return base.MouseDragEvent(p, rel, button, modifiers);
            }
        }

        public override bool MouseButtonEvent(Vector2 p, int button, bool down, int modifiers)
        {
            if(base.MouseButtonEvent(p, button, down, modifiers)) return true;

            if(down && button == SDL2.SDL.SDL_BUTTON_LEFT && Children.Count > 0 && 
                ChildPreferredHeight > Size.Y &&
                p.X > Position.X + Size.X - 13 &&
                p.X < Position.X + Size.X - 4) 
            {
                var scrollH = Height * MathF.Min(1, Height / ChildPreferredHeight);
                var start = Position.Y + 4 + 1 + (Size.Y - 8 - scrollH) * Scroll;

                var delta = 0f;

                if(p.Y < start){
                    delta = -Size.Y / ChildPreferredHeight;
                } else if(p.Y > start + scrollH) {
                    delta = Size.Y / ChildPreferredHeight;
                }

                Scroll = MathF.Max(0, MathF.Min(1, Scroll + delta * 0.98f));

                Children.First().Position = new Vector2(0, -Scroll * (ChildPreferredHeight - Size.Y));
                UpdateLayout = true;
                return true;
            }
            return false;
        }

        public override bool ScrollEvent(Vector2 p, Vector2 rel)
        {
            if(Children.Count > 0 && ChildPreferredHeight > Size.Y){
                var child = Children.First();
                float scrollAmount = rel.Y * Size.Y * .25f;

                Scroll = MathF.Max(0, MathF.Min(1, Scroll - scrollAmount / ChildPreferredHeight));
                
                var oldPos = child.Position;
                child.Position = new Vector2(0, -Scroll * (ChildPreferredHeight - Size.Y));
                var newPos = child.Position;
                UpdateLayout = true;
                child.MouseMotionEvent(p-Position, oldPos - newPos, 0, 0);

                return true;
            } else {
                return base.ScrollEvent(p, rel);
            }
        }

        public override void Draw(NVGcontext vg)
        {
            if(Children.Count == 0) return;
            var child = Children.First();
            var yOffset = 0f;
            if(ChildPreferredHeight > Size.Y) yOffset = -Scroll * (ChildPreferredHeight - Size.Y);
            child.Position = new Vector2(0, yOffset);
            ChildPreferredHeight = child.PreferredSize(vg).Y;
            var scrollH = Height * MathF.Min(1, Height / ChildPreferredHeight);

            if(UpdateLayout){
                UpdateLayout = false;
                child.PerformLayout(vg);
            }

            vg.Save();
            vg.Translate(Position.X, Position.Y);
            vg.IntersectScissor(0,0, Size.X, Size.Y);
            if(child.Visible) child.Draw(vg);
            vg.Restore();

            var paint = vg.BoxGradient(
                Position.X + Size.X - 12 + 1, 
                Position.Y + 4 + 1,
                8, Size.Y - 8, 3, 4, vg.RGBA(0,0,0,32), vg.RGBA(0,0,0,092));
            vg.BeginPath();
            vg.RoundedRect(Position.X + Size.X - 12, Position.Y + 4, 8, Size.Y - 8, 3);
            vg.FillPaint(paint);
            vg.Fill();
            vg.BoxGradient(
                Position.X + Size.X - 12 - 1, 
                Position.Y + 4 + (Size.Y - 8 - scrollH) * Scroll - 1,
                8, scrollH, 3, 4, vg.RGBA(220,220,220,100), vg.RGBA(128,128,128,100));

            vg.BeginPath();
            vg.RoundedRect(Position.X + Size.X - 12 + 1,
                Position.Y + 4 + 1 + (Size.Y - 8 - scrollH) * Scroll, 8 - 2, scrollH - 2, 2);
            vg.FillPaint(paint);
            vg.Fill();
        }
    }
}