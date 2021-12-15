using System;
using System.Numerics;
using net6test;

namespace MonoNanoGUI
{
    public class Screen : Widget
    {
        public Screen (IPlatform window, Vector2 size, string caption, bool resizable = true, bool fullscreen = false)
            : base (null)
        {
            InitializeWindow (window);
        }

        private void InitializeWindow (IPlatform window)
        {
            Console.WriteLine ("Initializing window.");    
        }

    }
}
