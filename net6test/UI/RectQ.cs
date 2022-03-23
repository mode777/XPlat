using System.Drawing;

namespace net6test.UI
{
    public class RectQ
    {
        public static implicit operator RectangleF(RectQ q) => q.ToRectangleF();

        public Quantity X;
        public Quantity Y;
        public Quantity Width;
        public Quantity Height;

        public RectQ(Quantity x, Quantity y, Quantity width, Quantity height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF ToRectangleF() => new RectangleF(X, Y, Width, Height);
    } 
}
