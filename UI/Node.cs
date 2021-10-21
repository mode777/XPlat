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
        private RectangleF bounds;

        public ICollection<Node> Children { get; } = new List<Node>();
        public RectangleF Bounds { get => bounds; }
        public bool HasMouseOver { get; private set; }
        public EventHandler OnClick { get; set; }

        public void SetPixelSize(float width, float height)
        {
            bounds.Width = width;
            bounds.Height = height;
        }

        public void SetPixelPos(float x, float y)
        {
            bounds.X = x;
            bounds.Y = y;
        }

        public virtual void Update(NVGcontext ctx)
        {            
            var m = IPlatformInfo.Default.MousePosition;
            HasMouseOver = bounds.Contains(m);
            if (HasMouseOver && IPlatformInfo.Default.MouseClicked)
                OnClick?.Invoke(this, null);

            foreach (var c in Children)
            {
                c.Update(ctx);
            }
        }

        //protected abstract SizeF CalculateSize(UiContext ctx);
        //protected abstract PointF CalculatePos(UiContext ctx);
        public abstract SizeF CalculateSize(UiContext ctx);
        public abstract void Arrange(UiContext ctx);
        public virtual void Draw(NVGcontext vg)
        {
            foreach (var c in Children)
            {
                c.Draw(vg);
            }
        }
    }
}