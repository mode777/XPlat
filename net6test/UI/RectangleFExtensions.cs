using System.Drawing;

namespace net6test.UI
{
    public static class RectangleFExtensions {
        public static RectangleF Expand(this RectangleF r, float offset) => new RectangleF(r.X - offset, r.Y - offset, r.Width + offset*2, r.Height + offset*2);
        public static RectangleF Expand(this RectangleF r, Thickness t) => new RectangleF(r.X - t.Left, r.Y - t.Top, r.Width + t.Left + t.Right, r.Height + t.Top + t.Bottom);
        public static RectangleF Inset(this RectangleF r, Thickness t) => new RectangleF(r.X + t.Left, r.Y + t.Top, r.Width - t.Left - t.Right, r.Height - t.Top - t.Bottom);
    }
}