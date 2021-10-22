using System.Drawing;

namespace net6test
{
    public interface IPlatformInfo
    {
        public static IPlatformInfo Default {  get; internal set; }
        Size RendererSize { get; }
        Size WindowSize { get; }
        Point MousePosition { get; }
        bool MouseClicked { get; }
        public float RetinaScale { get; }

        event EventHandler OnClick;
        event EventHandler OnResize;
        
        float SizeH(float val) => (val / 100f) * RendererSize.Width;
        float SizeV(float val) => (val / 100f) * RendererSize.Height;
        float PointSize(float val) => val * RetinaScale;
    }
}