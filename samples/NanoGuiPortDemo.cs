using GLES2;
using net6test.NanoGuiPort;
using NanoVGDotNet;
using System.Drawing;
using System.Numerics;

namespace net6test.samples
{
    public class NanoGuiPortDemo : Screen
    {
        public NanoGuiPortDemo(IPlatformInfo platform) : base(platform)
        {
              var window = new Window(this, "Button demo");
              window.Position = new Vector2(15,15);
              //window.Layout = new GroupLayout();

              //new Label(window, "Push buttons", "sans-bold");

            PerformLayout();
        }
    }
}