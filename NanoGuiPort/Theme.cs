using NanoVGDotNet;

namespace net6test.NanoGuiPort
{
    public class Theme
    {
        private NVGcontext nvgContext;

        public Theme(NVGcontext nvgContext)
        {
            this.nvgContext = nvgContext;
        }

        public float IconScale { get; internal set; } = 0.6f;
        public float StandardFontSize { get; internal set; } = 16;
        public float WindowDropShadowSize { get; internal set; } = 10;
        public float WindowCornerRadius { get; internal set; } = 2;
        public float WindowHeaderHeight { get; internal set; } = 30;
        public NVGcolor WindowFillUnfocused { get; internal set; } = "#2B2B2BE6"; 
        public NVGcolor WindowFillFocused { get; internal set; } = "#2D2D2DE6";
    }
}