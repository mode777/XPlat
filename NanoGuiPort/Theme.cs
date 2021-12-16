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
        public NVGcolor DropShadow { get; internal set; } = "#00000088";
        public NVGcolor Transparent { get; internal set; } = "#00000000";
        public NVGcolor WindowHeaderGradientTop => ButtonGradientTopUnfocused;
        public NVGcolor WindowHeaderGradientBottom => ButtonGradientBotUnfocused;
        public NVGcolor ButtonGradientTopFocused { get; set; } = NanoVG.nvgRGBA(64, 64, 64, 255);
        public NVGcolor ButtonGradientBotFocused { get; set; } = NanoVG.nvgRGBA(48, 48, 48, 255);
        public NVGcolor ButtonGradientTopUnfocused { get; set; } = NanoVG.nvgRGBA(74, 74, 74, 255);
        public NVGcolor ButtonGradientBotUnfocused { get; set; } = NanoVG.nvgRGBA(58, 58, 58, 255);
        public NVGcolor WindowHeaderSepTop => BorderLight;
        public NVGcolor WindowHeaderSepBot => BorderDark;
        public NVGcolor BorderDark { get; set; } = NanoVG.nvgRGBA(29,29,29,255);
        public NVGcolor BorderLight { get; set; } = NanoVG.nvgRGBA(92,92,92,255);
        public NVGcolor WindowTitleFocused { get; set; } = NanoVG.nvgRGBA(255, 255, 255, 190);
        public NVGcolor WindowTitleUnfocused { get; set; } = NanoVG.nvgRGBA(220, 220, 220, 160);
        public NVGcolor TextColor { get; internal set; } = NanoVG.nvgRGBA(255, 255, 255, 160);
    }
}