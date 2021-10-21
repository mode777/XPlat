using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class UiContext
    {
        public NVGcontext Vg;
        public float? X;
        public float? Y;
        public float? MaxX;
        public float? MinX;

        public UiContext Clone() => (UiContext)MemberwiseClone();
    }

    public abstract class Node
    {
        public ICollection<Node> Children { get; } = new List<Node>();
        public RectangleF Bounds { get; private set; }
        public bool HasMouseOver { get; private set; }
        public EventHandler OnClick { get; set; }

        public virtual void Update(UiContext ctx)
        {
            Bounds = CalculateBounds(ctx);
            foreach (var c in Children)
            {
                c.Update(ctx);
            }

            var m = IPlatformInfo.Default.MousePosition;
            HasMouseOver = Bounds.Contains(m);
            if (HasMouseOver && IPlatformInfo.Default.MouseClicked)
                OnClick?.Invoke(this, null);
        }

        //protected abstract SizeF CalculateSize(UiContext ctx);
        //protected abstract PointF CalculatePos(UiContext ctx);
        protected abstract RectangleF CalculateBounds(UiContext ctx);
    }
}