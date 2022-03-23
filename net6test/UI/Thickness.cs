namespace net6test.UI
{
    public struct Thickness
    {
        public static implicit operator Thickness(Quantity q) => new Thickness(q, q, q, q);
        public static implicit operator Thickness(string str) 
        {
            var split = str.Split(' ');
            switch(split.Length)
            {
                case 1:
                    return new Thickness(split[0],split[0],split[0],split[0]);
                case 2:
                    return new Thickness(split[0],split[1],split[0],split[1]);
                case 4:
                    return new Thickness(split[1],split[2],split[3],split[4]);
                default:
                    throw new Exception("Unable to parse thickness");

            }
        }

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
    }
}