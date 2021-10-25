using NanoVGDotNet;

namespace net6test.UI
{
    public class Style
    {
        static Style()
        {
            Default = new Style
            {
                FontColor = "#000000",
            };
        }

        public static Style Default { get; private set; }

        public Style(Style? parent = null)
        {
            this.parent = parent;
        }

        private FillStyle? fill;
        private Quantity? cornerRadius;
        private Shadow? shadow;
        //private Thickness? padding;
        private NVGcolor? fontColor;
        private Shadow? fontShadow;

        private readonly Style? parent;

        public FillStyle? Fill { get => fill ?? parent?.Fill; set => fill = value; }
        public Quantity? CornerRadius { get => cornerRadius ?? parent?.CornerRadius; set => cornerRadius = value; }
        public Shadow? Shadow { get => shadow ?? parent?.Shadow; set => shadow = value; }
        //public Thickness? Padding { get => padding ?? parent?.Padding ?? Default.Padding; set => padding = value; }
        public NVGcolor? FontColor { get => fontColor ?? parent?.FontColor; set => fontColor = value; }
        public Shadow? FontShadow { get => fontShadow ?? parent?.FontShadow; set => fontShadow = value; }
    }
}