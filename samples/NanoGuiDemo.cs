using GLES2;
using MonoNanoGUI;
using NanoVGDotNet;
using System.Drawing;
using System.Numerics;

namespace net6test.samples
{
    public class NanoGuiDemo : ISdlApp
    {
        private NVGcontext vg;
        private int font;
        private int img;
        private float fsize = 12;
        private Widget screen;
        private readonly IPlatformInfo platform;

        public NanoGuiDemo(IPlatformInfo platform)
        {
            this.platform = platform;

        }

        public void Init()
        {
            vg = new NVGcontext();
            GlNanoVG.nvgCreateGL(ref vg, (int)NVGcreateFlags.NVG_ANTIALIAS |
                        (int)NVGcreateFlags.NVG_STENCIL_STROKES);

            vg.CreateFont("sans", "assets/Roboto-Regular.ttf");
            vg.CreateFont("sans-bold", "assets/Roboto-Bold.ttf");
            vg.CreateFont("icons", "assets/entypo.ttf");

            this.screen = new Widget()
                .WithLocalPosition(Vector2.Zero)
                .WithSize(new Vector2(platform.RendererSize.Width, platform.RendererSize.Height));

            Window window = screen.AddNewWidget<Window>();
            window.WithTitle("Button demo")
                  .WithLocalPosition(new Vector2(15f, 50f))
                  .WithSize(new Vector2(250f, 400f))
                  .WithLayout(new GroupLayout());

            // -- Push buttons
            window.AddNewWidget<Label>()
                  .WithCaption("Push buttons")
                  .WithFont("sans-bold");

            window.AddNewWidget<Button>()
                  .WithCaption("Plain button")
                  .WithClickCallback((btn) => Console.WriteLine("Click!"));

            window.AddNewWidget<Button>()
                  .WithCaption("Styled")
                  .WithIcon((int)MonoNanoGUI.Font.Entypo.ICON_ROCKET, Button.IconAnchorType.LeftCentered);

            // -- Toggle buttons
            window.AddNewWidget<Label>()
                  .WithCaption("Toggle buttons")
                  .WithFont("sans-bold");

            window.AddNewWidget<Button>()
                  .WithFlags(Button.Flags.ToggleButton)
                  .WithCaption("Toggle me");

            // -- Radio buttons
            window.AddNewWidget<Label>()
                  .WithCaption("Radio buttons")
                  .WithFont("sans-bold");

            window.AddNewWidget<Button>()
                  .WithCaption("Radio button 1")
                  .WithFlags(Button.Flags.RadioButton);

            window.AddNewWidget<Button>()
                  .WithCaption("Radio button 2")
                  .WithFlags(Button.Flags.RadioButton);

            // -- Tool buttons palette
            window.AddNewWidget<Label>()
                  .WithCaption("A tool palette")
                  .WithFont("sans-bold");

            Widget tools = window.AddNewWidget<Widget>()
                                 .WithLayout(new BoxLayout(Layout.Orientation.Horizontal, Layout.Alignment.Middle, 0, 6));
            tools.AddChild(Button.MakeToolButton((int)MonoNanoGUI.Font.Entypo.ICON_CLOUD));
            tools.AddChild(Button.MakeToolButton((int)MonoNanoGUI.Font.Entypo.ICON_FF));
            tools.AddChild(Button.MakeToolButton((int)MonoNanoGUI.Font.Entypo.ICON_COMPASS));
            tools.AddChild(Button.MakeToolButton((int)MonoNanoGUI.Font.Entypo.ICON_INSTALL));

            // -- Popup buttons
            window.AddNewWidget<Label>()
                  .WithCaption("Popup buttons")
                  .WithFont("sans-bold");

            PopupButton pButton = window.AddNewWidget<PopupButton>()
                                        .WithIcon((int)MonoNanoGUI.Font.Entypo.ICON_EXPORT)
                                        .WithCaption("Popup")
                                        as PopupButton;
            pButton.popup.WithLayout(new GroupLayout());
            pButton.popup.AddNewWidget<Label>()
                   .WithCaption("Arbitrary widgets can be placed here");
            pButton.popup.AddNewWidget<CheckBox>()
                   .WithCaption("A check box");

            PopupButton rpButton = pButton.popup.AddNewWidget<PopupButton>()
                                          .WithIcon((int)MonoNanoGUI.Font.Entypo.ICON_FLASH)
                                          .WithCaption("Recursive popup")
                                          as PopupButton;
            rpButton.popup.WithLayout(new GroupLayout());
            rpButton.popup.AddNewWidget<CheckBox>()
                    .WithCaption("Another check box");

            screen.PerformLayout (vg);
        }

        public void Update()
        {

            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);
            vg.Scale(platform.RetinaScale, platform.RetinaScale);
            screen.Draw(vg);

            vg.EndFrame();
        }
    }
}