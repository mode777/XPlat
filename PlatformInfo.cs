using System.Drawing;

namespace net6test
{
    public class PlatformInfo : IPlatformInfo
    {
        public Size RendererSize { get; set; }
        public Size WindowSize { get; set; }
        public Point MousePosition { get; set; }

        public float RetinaScale => RendererSize.Width / (float)WindowSize.Width;

        public bool MouseClicked { get; set; }

        public event EventHandler? OnClick;

        public void RaiseOnClick() => OnClick?.Invoke(this, null);
    }
}