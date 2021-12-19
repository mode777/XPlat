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
        private Label label3;
        private Window window2;
        private Button button;

        public NanoGuiPortDemo(ILogger<NanoGuiPortDemo> logger, ISdlPlatformEvents events, IPlatform info) : base(info, events)
        {
            this.logger = logger;
            this.events = events;

            this.window = new Window(this, "Metrics Panel");
            window.Position = new Vector2(15,15);
            window.Layout = new GroupLayout();

            this.label1 = new Label(window, "", "sans-bold");
            this.label2 = new Label(window, "", "sans-bold");
            this.label3 = new Label(window, "", "sans-bold");

            new Label(window, "Push Buttons");
            button = new Button(window, "Plain Button");
            button.Tooltip = "Short tooltip";
            button = new Button(window, "Styled", (int)Icons.FA_ROCKET);
            button.BackgroundColor = "#0000FF19";
            button.Tooltip = "This button has a fairly long tooltip. It is so long, in fact, that the shown text will span several lines.";
            button.OnPush += (s,a) => button.Tooltip = "Button clicked";

            new Label(window, "Toggle buttons", "sans-bold");
            var b = new Button(window, "Toggle me"){
                Flags = ButtonFlags.ToggleButton
            };
            b.OnChange += (s,a) => b.Caption = a ? "On" : "Off";


            
            UpdateValues();
            PerformLayout();
        }

        public void UpdateValues()
        {
            label1.Caption = "Mouse Position " + MousePos.ToString();
            label2.Caption = "Runtime(s) " + ((int)Time.RunningTime).ToString();
            label3.Caption = "Focused " + button.MouseFocus;
        }


        public override void Update()
        {
            UpdateValues();
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