using System.Drawing;

namespace net6test
{
    public class PlatformInfo : IPlatformInfo
    {
        public Size RendererSize { get; set; }
        public Size WindowSize { get; set; }
        public Point MousePosition { get; set; }

        public SizeF Scale => new SizeF(RendererSize.Width / (float)WindowSize.Width, RendererSize.Height / (float)WindowSize.Height);

        public bool MouseClicked { get; set; }

        public event EventHandler? OnClick;

        public void RaiseOnClick() => OnClick?.Invoke(this, null);
    }
}