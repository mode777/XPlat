using XPlat.NanoVg;

namespace XPlat.NanoGui
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
        public NVGcolor ButtonGradientTopFocused { get; set; } = NanoVgApi.nvgRGBA(64, 64, 64, 255);
        public NVGcolor ButtonGradientBotFocused { get; set; } = NanoVgApi.nvgRGBA(48, 48, 48, 255);
        public NVGcolor ButtonGradientTopUnfocused { get; set; } = NanoVgApi.nvgRGBA(74, 74, 74, 255);
        public NVGcolor ButtonGradientBotUnfocused { get; set; } = NanoVgApi.nvgRGBA(58, 58, 58, 255);
        public NVGcolor ButtonGradientTopPushed { get; set; } = NanoVgApi.nvgRGBA(41, 41, 41, 255);
        public NVGcolor ButtonGradientBotPushed { get; set; } = NanoVgApi.nvgRGBA(29, 29, 29, 255);
        public NVGcolor WindowHeaderSepTop => BorderLight;
        public NVGcolor WindowHeaderSepBot => BorderDark;
        public NVGcolor BorderDark { get; set; } = NanoVgApi.nvgRGBA(29,29,29,255);
        public NVGcolor BorderLight { get; set; } = NanoVgApi.nvgRGBA(92,92,92,255);
        public NVGcolor WindowTitleFocused { get; set; } = NanoVgApi.nvgRGBA(255, 255, 255, 190);
        public NVGcolor WindowTitleUnfocused { get; set; } = NanoVgApi.nvgRGBA(220, 220, 220, 160);
        public NVGcolor TextColor { get; set; } = NanoVgApi.nvgRGBA(255, 255, 255, 160);
        public float ButtonFontSize { get; set; } = 20;
        public float ButtonCornerRadius { get; internal set; } = 2;
        public NVGcolor DisabledTextColor { get; internal set; } = NanoVgApi.nvgRGBA(255,255,255,80);
        public NVGcolor TextColorShadow { get; internal set; } = NanoVgApi.nvgRGBA(0,0,0,160);
        public int PopupChevronRightIcon { get; internal set; } = (int)Icons.FA_CHEVRON_RIGHT;
        public int PopupChevronLeftIcon { get; internal set; } = (int)Icons.FA_CHEVRON_LEFT;
        public NVGcolor WindowPopup { get; internal set; } = NanoVgApi.nvgRGBA(50,50,50,255);
        public NVGcolor WindowPopupTransparent { get; internal set; } = NanoVgApi.nvgRGBA(50,50,50,0);
        public NVGcolor IconColor => TextColor;
        public int CheckBoxIcon { get; internal set; } = (int)Icons.FA_CHECK;
        public int InformationIcon { get; set; } = (int)Icons.FA_INFO_CIRCLE;
        public int QuestionIcon { get; set; } = (int)Icons.FA_QUESTION_CIRCLE;
        public int WarningIcon { get; set; } = (int)Icons.FA_EXCLAMATION_CIRCLE;
        public int MessageAltButtonIcon { get; set; } = (int)Icons.FA_TIMES_CIRCLE;
        public int MessagePrimaryButtonIcon { get; set; } = (int)Icons.FA_CHECK;
        public int TextBoxUpIcon { get; set; } = (int)Icons.FA_CHEVRON_UP;
        public int TextBoxDownIcon { get; set; } = (int)Icons.FA_CHEVRON_DOWN;
    }
}