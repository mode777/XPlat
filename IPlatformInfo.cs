using System.Drawing;

namespace net6test
{
    public interface IPlatformInfo
    {
        Size RendererSize { get; }
        Size WindowSize { get; }
        Point MousePosition { get; }
        bool MouseClicked { get; }
        event EventHandler OnClick;
        float SizeH(float val) => (val / 100f) * RendererSize.Width;
        float SizeV(float val) => (val / 100f) * RendererSize.Height;
        float Size(float val) => (val / 100f) * Math.Min(RendererSize.Height, RendererSize.Width);
    }
}