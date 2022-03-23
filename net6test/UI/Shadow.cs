using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class Shadow 
    {
        public Shadow(Quantity x, Quantity y, Quantity size, NVGcolor color)
        {
            Offset = new(x,y);
            Size = size;
            Color = color;
        }

        public PointQ Offset { get; set; }
        public Quantity Size { get; set; }
        public NVGcolor Color { get; set; }
    }
}