using System.Drawing;

namespace net6test.UI
{
    public class SizeQ
    {
        public static implicit operator SizeF(SizeQ q) => q.ToPointF();

        public Quantity Width;
        public Quantity Height;

        public SizeQ(Quantity width, Quantity height)
        {
            Width = width;
            Height = height;
        }

        public SizeF ToPointF() => new SizeF(Width, Height);
    }
}
