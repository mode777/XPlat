using GLES2;
using net6test.NanoGuiPort;
using NanoVGDotNet;
using System.Drawing;
using System.Numerics;
using Microsoft.Extensions.Logging;
using static SDL2.SDL;

namespace net6test.samples
{
    public class NanoGuiPortDemo : Screen
    {
        private readonly ILogger<NanoGuiPortDemo> logger;
        private readonly ISdlPlatformEvents events;
        private readonly Window window;
        private Label label;
        private Label label1;
        private Label label2;

        public NanoGuiPortDemo(ILogger<NanoGuiPortDemo> logger, ISdlPlatformEvents events, IPlatform info) : base(info, events)
        {
            this.logger = logger;
            this.events = events;

            this.window = new Window(this, "Metrics Panel");
            window.Position = new Vector2(15,15);
              //window.Position = new Vector2(0,0);
            //window.Size = new Vector2(300,300);
            window.FixedWidth = 300;
            window.Layout = new GroupLayout();

            this.label1 = new Label(window, "", "sans-bold");
            this.label2 = new Label(window, "", "sans-bold");

            PerformLayout();
        }


        public override void Update()
        {
            label1.Caption = "Mouse Position " + MousePos.ToString();
            label2.Caption = "Runtime(s) " + ((int)Time.RunningTime).ToString();
            base.Update();
        }

        public override void DrawContents()
        {
            base.DrawContents();
            var vg = nvgContext;
            vg.BeginPath();
            vg.Circle(Platform.MousePosition.X, Platform.MousePosition.Y, 2);
            vg.FillColor("#ff0000");
            vg.Fill();
        }
    }
}