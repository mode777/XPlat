using NanoVGDotNet;

namespace net6test.UI
{
    public class UiContext
    {
        public NVGcontext Vg;
        public float? X;
        public float? Y;
        public float? MaxW;
        public float? MinW;
        public float? MaxH;
        public float? MinH;
        public float? Width { get; set; }
        public float? Height { get; set; }

        public UiContext Clone() => (UiContext)MemberwiseClone();
    }
}