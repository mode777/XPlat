using System.Diagnostics;
using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{

    public abstract class Element
    {
        private RectangleF bounds;

        public ICollection<Element> Children { get; } = new List<Element>();
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
            if(Debugger.IsAttached)
                DrawDebugRect(vg);
            foreach (var c in Children)
            {
                c.Draw(vg);
            }
        }

        private void DrawDebugRect(NVGcontext vg)
        {
            vg.BeginPath();
            vg.Rect(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            if(HasMouseOver){
                // vg.FillColor("#00ffff44");
                // vg.Fill();
                vg.StrokeColor("#00ffff");
            } else {
                vg.StrokeColor("#ff00ff");
            }
            vg.Stroke();
        }
    }
}