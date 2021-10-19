using NanoVGDotNet;

namespace net6test.UI
{
    public class Style
    {
        public NVGcolor TextColor { get;set; }
        public FillStyle Background { get;set; }
        public float FontSize { get;set; }
        public ShadowStyle Shadow { get; set; }
        public Thickness Margin { get; set; }
        public Thickness Padding { get; set; }
        public Quantity Width { get; set; } = "100%";
        public Quantity Height { get; set; } = "100%";
        public float BorderRadius { get; set; }
        public int ZIndex { get; set; } = 0;
    }
}
