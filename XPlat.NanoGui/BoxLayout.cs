using System;
using System.Numerics;
using XPlat.NanoVg;

namespace XPlat.NanoGui
{
    public class BoxLayout : Layout
    {
        public BoxLayout(Orientation orientation, Alignment alignment, float margin, float spacing)
        {
            Orientation = orientation;
            Alignment = alignment;
            Margin = margin;
            Spacing = spacing;
        }

        public Orientation Orientation { get; set; }
        public Alignment Alignment { get; set; }
        public float Margin { get; set; }
        public float Spacing { get; set; }

        public override void PerformLayout(NVGcontext ctx, Widget widget)
        { 
            var fsW = widget.FixedSize;
            var containerSize = new Vector2(
                fsW.X == 0 ? widget.Width : fsW.X,
                fsW.Y == 0 ? widget.Height : fsW.Y);

            int axis1 = Orientation == Orientation.Horizontal ? 0 : 1; 
            int axis2 = Orientation == Orientation.Horizontal ? 1 : 0; 
            var position = Margin;
            float yOffset = 0;

            if(widget is Window window && !string.IsNullOrEmpty(window.Title)){
                if(Orientation == Orientation.Vertical){
                    position += widget.Theme.WindowHeaderHeight - Margin / 2;
                } else {
                    yOffset = widget.Theme.WindowHeaderHeight;
                    containerSize.Y -= yOffset;
                }
            }

            bool first = true;
            foreach (var w in widget.Children)
            {
                if(!w.Visible) continue;
                if(first) first = false;
                else position += Spacing;

                var ps = w.PreferredSize(ctx);
                var fs = w.FixedSize;

                var targetSize = new Vector2(fs.X == 0 ? ps.X : fs.X, fs.Y == 0 ? ps.Y : fs.Y);
                var pos = new Vector2(0, yOffset);

                pos.Component(axis1, position);

                switch (Alignment)
                {
                    case Alignment.Min:
                        pos.Component(axis2, pos.Component(axis2) + Margin);
                        break;
                    case Alignment.Middle:
                        pos.Component(axis2, pos.Component(axis2) + (containerSize.Component(axis2) - targetSize.Component(axis2)) / 2);
                        break;
                    case Alignment.Max:
                        pos.Component(axis2, pos.Component(axis2) + containerSize.Component(axis2) - targetSize.Component(axis2) - Margin * 2);
                        break;
                    case Alignment.Fill:
                        pos.Component(axis2, pos.Component(axis2) + Margin);
                        targetSize.Component(axis2, fs.Component(axis2) != 0 ? fs.Component(axis2) : (containerSize.Component(axis2) - Margin * 2));
                        break;
                }

                w.Position = pos;
                w.Size = targetSize;
                w.PerformLayout(ctx);
                position += targetSize.Component(axis1);
            }

        }

        public override Vector2 PreferredSize(NVGcontext ctx, Widget widget)
        {
            var size = new Vector2(Margin * 2, Margin * 2);

            float yOffset = 0;
            if(widget is Window window && !string.IsNullOrEmpty(window.Title)){
                if(Orientation == Orientation.Vertical){
                    size.Y += widget.Theme.WindowHeaderHeight - Margin / 2f;
                } else {
                    yOffset = widget.Theme.WindowHeaderHeight;
                }
            }

            bool first = true;
            int axis1 = Orientation == Orientation.Horizontal ? 0 : 1; 
            int axis2 = Orientation == Orientation.Horizontal ? 1 : 0; 
            foreach (var w in widget.Children)
            {
                if(!w.Visible) continue;
                if(first) first = false;
                else size.Component(axis1, size.Component(axis1) + Spacing);

                var ps = w.PreferredSize(ctx);
                var fs = w.FixedSize;

                var targetSize = new Vector2(fs.X == 0 ? ps.X : fs.X, fs.Y == 0 ? ps.Y : fs.Y);

                size.Component(axis1, size.Component(axis1) + targetSize.Component(axis1));
                size.Component(axis2, MathF.Max(size.Component(axis2), targetSize.Component(axis2) + 2 * Margin));

                first = false;
            }
            return size + new Vector2(0, yOffset);
        }
    }
}