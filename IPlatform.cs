using System.Drawing;

namespace net6test
{

    public interface IPlatform
    {
        //public static IPlatformInfo Default {  get; internal set; }
        bool AutoSwap { get; set; }
        Size RendererSize { get; }
        Size WindowSize { get; set; }
        Point MousePosition { get; }
        //bool MouseClicked { get; }
        float RetinaScale { get; }

        string WindowTitle { get; set; }
        void SwapBuffers();
        

        //event EventHandler OnClick;
        //event EventHandler OnResize;
        // event EventHandler MouseMove;
        // event EventHandler TextInput;



        //event EventHandler<SDL2.SDL.SDL_Keycode>? OnKeyUp;

        //float SizeH(float val) => (val / 100f) * RendererSize.Width;
        //float SizeV(float val) => (val / 100f) * RendererSize.Height;
        //float PointSize(float val) => val * RetinaScale;
    }
}