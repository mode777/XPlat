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
    }
}