using System.Numerics;
using NanoVGDotNet;

namespace net6test.NanoGuiPort
{
    public class GroupLayout : Layout
    {
        public GroupLayout(float margin = 15, float spacing = 6, float groupSpacing = 14, float groupIndent = 20)
        {
            Margin = margin;
            Spacing = spacing;
            GroupSpacing = groupSpacing;
            GroupIndent = groupIndent;
        }

        public float Margin { get; set; }
        public float Spacing { get; set; }
        public float GroupSpacing { get; set; }
        public float GroupIndent { get; set; }

        public override void PerformLayout(NVGcontext ctx, Widget widget)
        {
            float height = Margin;
            float availableWidth = (widget.FixedWidth != 0 ? widget.FixedWidth : widget.Width - 2 * Margin);

            if(widget is Window window && !string.IsNullOrEmpty(window.Title)){
                height += widget.Theme?.WindowHeaderHeight ?? 0 - Margin/2;
            }

            bool first = true;
            bool indent = false;
            foreach (var c in widget.Children)
            {
                if(!c.Visible) continue;
                var label = c as Label;
                if(!first) height += label != null ? Spacing : GroupSpacing;
                first = false;
                
                bool indentCur = indent && label == null;
                var ps = new Vector2(availableWidth - (indentCur ? GroupIndent : 0), c.PreferredSize(ctx).Y);
                var fs = c.FixedSize;
                
                var targetSize = new Vector2(fs.X==0 ? ps.X : fs.X, fs.Y==0 ? ps.Y : fs.Y);

                c.Position = new Vector2(Margin + (indentCur ? GroupIndent: 0), height);
                c.Size = targetSize;
                c.PerformLayout(ctx);

                height += targetSize.Y;

                if(label != null) indent = !String.IsNullOrEmpty(label.Caption);
            }
        }

        public override Vector2 PreferredSize(NVGcontext ctx, Widget widget)
        {
            float height = Margin;
            float width = 2 * Margin;

            if(widget is Window window && !string.IsNullOrEmpty(window.Title)){
                height += widget.Theme?.WindowHeaderHeight ?? 0 - Margin/2;
            }

            bool first = true;
            bool indent = false;
            foreach (var c in widget.Children)
            {
                if(!c.Visible) continue;
                var label = c as Label;
                if(!first) height += label != null ? Spacing : GroupSpacing;
                first = false;
                
                var ps = c.PreferredSize(ctx);
                var fs = c.FixedSize;
                var targetSize = new Vector2(fs.X==0 ? ps.X : fs.X, fs.Y==0 ? ps.Y : fs.Y);

                bool indentCur = indent && label == null;
                height += targetSize.Y;
                width = MathF.Max(width, targetSize.X + 2 * Margin + (indentCur ? GroupIndent : 0));

                if(label != null) indent = !String.IsNullOrEmpty(label.Caption);
            }
            height += Margin;
            return new Vector2(width, height);
        }
    }
}