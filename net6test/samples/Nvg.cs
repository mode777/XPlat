using GLES2;
using NanoVGDotNet;
using System.Drawing;
using net6test.UI;

namespace net6test.samples
{
    public class Nvg : ISdlApp
    {
        private NVGcontext vg;
        private int font;
        private int img;
        private Layout layout;
        private Panel panel;
        private float fsize = 12;
        private readonly IPlatform platform;

        public Nvg(IPlatform platform)
        {
            this.platform = platform;

        }

        public void Init()
        {
            vg = new NVGcontext();
            GlNanoVG.nvgCreateGL(ref vg, (int)NVGcreateFlags.NVG_ANTIALIAS |
                        (int)NVGcreateFlags.NVG_STENCIL_STROKES);
            vg.CreateFont("sans", "assets/Roboto-Regular.ttf");
            vg.CreateFont("serif", "assets/Merriweather-Regular.ttf");
            img = vg.CreateImage("assets/bamberg.png", 0);

            layout = new Layout(vg);
            panel = new Panel();
            panel.Style.Fill = "#00000088";
            panel.Width = "33vw";
            panel.X = "60vw";
            panel.Style.Shadow = new Shadow(0,0,"1vh", "#000000");
            layout.Children.Add(panel);
            layout.Arrange();
        }

        public void Update()
        {
            layout.Arrange();
            layout.Update();

            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);

            vg.BeginPath();
            vg.Rect(0,0,(Quantity)"100vw", (Quantity)"100vh");
            vg.FillColor("#F6D7A7");
            vg.Fill();

            vg.BeginPath();
            vg.Rect(0,0,(Quantity)"100vw",(Quantity)"100vh");
            var p = vg.ImagePattern((Quantity)"-10vw",(Quantity)"0vh",(Quantity)"111vw",(Quantity)"101vh",0,img,1);
            vg.FillPaint(p);
            vg.Fill();

            layout.Draw();

            vg.EndFrame();
        }
    }
}