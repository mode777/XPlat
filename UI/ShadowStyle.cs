using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public class ShadowStyle
    {
        public NVGpaint Paint { get; set; }
        Quantity Size { get; set; }
        PointF Offset { get; set; }
    }
}
