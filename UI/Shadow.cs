using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class Shadow 
    {
        public Shadow(float x, float y, float size, NVGcolor color)
        {
            Offset = new(x,y);
            Size = size;
            Color = color;
        }

        public PointF Offset { get; set; }
        public float Size { get; set; }
        public NVGcolor Color { get; set; }
        internal PointF OffsetScaled { get; private set; }
        internal float SizeScaled { get; set; }

        internal void Update(IPlatformInfo p){
            OffsetScaled = new(p.SizeH(Offset.X),p.SizeV(Offset.Y));
            SizeScaled = p.Size(Size);
        }
    }
}