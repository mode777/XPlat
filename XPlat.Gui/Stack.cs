using System.Numerics;
using XPlat.Core;
using XPlat.NanoVg;

namespace XPlat.Gui
{
    public class Stack : Widget
    {
        public Stack(Widget? parent = null) : base(parent)
        {
        }

        public Direction Direction { get; set; }

        public override Vector2 PreferredSize(NVGcontext ctx)
        {
            var expand = Direction == Direction.Horizontal ? 0 : 1;
            var stack = Direction == Direction.Horizontal ? 1 : 0;

            var size = Vector2.Zero;
            foreach (var c in Children)
            {
                var csize = c.PreferredSize(ctx);
                size.Component(stack, Math.Max(size.Component(stack), csize.Component(stack)));
                size.Component(expand, size.Component(expand) + csize.Component(expand));
            }

            return size;
        }

        public override void PerformLayout(NVGcontext ctx)
        {
            var pref = PreferredSize(ctx);
            var fix = FixedSize;
            Size = new Vector2(
                fix.X != 0 ? fix.X : pref.X,
                fix.Y != 0 ? fix.Y : pref.Y);

            var expand = Direction == Direction.Horizontal ? 0 : 1;

            var pos = Vector2.Zero;
            foreach (var c in Children)
            {
                var csize = c.PreferredSize(ctx);
                c.Position = pos;
                pos.Component(expand, pos.Component(expand) + csize.Component(expand));
                c.PerformLayout(ctx);
            }
        }


    }
}