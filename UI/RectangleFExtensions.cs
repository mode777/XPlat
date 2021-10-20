using System.Drawing;

namespace net6test.UI
{
    public static class RectangleFExtensions {
        public static RectangleF Expand(this RectangleF r, float offset) => new RectangleF(r.X - offset, r.Y - offset, r.Width + offset*2, r.Height + offset*2);
    }
}