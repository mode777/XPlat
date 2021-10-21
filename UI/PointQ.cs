using System.Drawing;

namespace net6test.UI
{
    public class PointQ
    {
        public static implicit operator PointF(PointQ q) => q.ToPointF();
        
        public Quantity X;
        public Quantity Y;

        public PointQ(Quantity x, Quantity y)
        {
            X = x;
            Y = y;
        }

        public PointF ToPointF() => new PointF(X, Y);
    }
}
