using System;
using System.Collections.Generic;
using System.Text;

namespace XPlat.Core
{
    public static class Window
    {
        static internal IPlatform Platform;

        public static int Width => (int)Platform.WindowSize.X;            
        public static int Height => (int)Platform.WindowSize.Y;
        public static int RendererWidth => (int)Platform.RendererSize.X;
        public static int RendererHeight => (int)Platform.RendererSize.Y;

    }
}
