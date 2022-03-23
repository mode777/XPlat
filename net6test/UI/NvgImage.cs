using NanoVGDotNet;

namespace net6test.UI
{
    public class NvgImage
    {
        public static NvgImage FromFile(NVGcontext vg, string filename){
            var h = vg.CreateImage(filename, 0);
            int x = 0,y = 0;
            vg.ImageSize(h, ref x, ref y);
            return new NvgImage(h, x,y);
        }

        private NvgImage(int handle, int width, int height)
        {
            Handle = handle;
            Width = width;
            Height = height;
        }

        public int Handle { get; }
        public int Width { get; }
        public int Height { get; }
    }
}