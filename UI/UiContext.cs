using NanoVGDotNet;

namespace net6test.UI
{
    public class UiContext
    {
        public NVGcontext Vg;
        public float? X;
        public float? Y;
        public float? MaxX;
        public float? MinX;

        public UiContext Clone() => (UiContext)MemberwiseClone();
    }
}