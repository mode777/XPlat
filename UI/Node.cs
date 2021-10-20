using System.Drawing;

namespace net6test.UI
{
    public class Node 
    {
        public RectangleF Rect;
        public RectangleF Bounds { get; private set; }
        public bool HasMouseOver { get; private set; }
        public EventHandler OnClick { get; set; }
        
        public virtual void UpdateBounds(IPlatformInfo ctx)
        {
            Bounds = new RectangleF(ctx.SizeH(Rect.X), ctx.SizeV(Rect.Y), ctx.SizeH(Rect.Width), ctx.SizeV(Rect.Height));
            HasMouseOver = Bounds.Contains(ctx.MousePosition);
            if(HasMouseOver && ctx.MouseClicked)
                OnClick?.Invoke(this, null);
        }
    }
}