namespace net6test.UI
{
    public struct Thickness
    {
        public static implicit operator Thickness(float f) => new Thickness(f, f, f, f);

        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public Thickness(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Thickness ToScaled(IPlatformInfo p)
        {
            return new Thickness(p.Size(Left), p.Size(Top), p.Size(Right), p.Size(Bottom));
        }
    }
}