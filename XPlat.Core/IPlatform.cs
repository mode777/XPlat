using System.Numerics;

namespace XPlat.Core
{

    public interface IPlatform
    {
        int TargetFramerate { get; set; }
        bool AutoSwap { get; set; }
        bool AutoClear { get; set; }
        Vector2 RendererSize { get; }
        Vector2 WindowSize { get; set; }
        Vector2 MousePosition { get; }
        float RetinaScale { get; }
        Vector3 ClearColor { get; }
        string WindowTitle { get; set; }
        void SwapBuffers();
        void Clear();
    }
}