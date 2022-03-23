using System.Numerics;

namespace XPlat.Core
{

    public interface IPlatform
    {
        int TargetFramerate { get; set; }
        bool AutoSwap { get; set; }
        Vector2 RendererSize { get; }
        Vector2 WindowSize { get; set; }
        Vector2 MousePosition { get; }
        float RetinaScale { get; }

        string WindowTitle { get; set; }
        void SwapBuffers();
    }
}