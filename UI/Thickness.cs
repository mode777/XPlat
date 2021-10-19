namespace net6test.UI
{
    public struct Thickness
    {
        public Quantity Left;
        public Quantity Top;
        public Quantity Right;
        public Quantity Bottom;

        public Thickness(Quantity left, Quantity top, Quantity right, Quantity bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Thickness(Quantity margin) : this(margin,margin,margin,margin)
        {
        }
    }
}
