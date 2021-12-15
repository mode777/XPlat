using GLES2;
using NanoVGDotNet;
using System.Drawing;
using net6test.UI;
using net6test.Maps;

namespace net6test.samples
{
    public class Map : ISdlApp
    {
        private NVGcontext vg;
        private int font;
        private CityMap map;
        private Layout layout;
        private readonly IPlatform platform;
        private float r = 0;

        public Map(IPlatform platform)
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

            var json = GeoJson.Load("assets/thinfishcy.json");
            map = new CityMap(json as FeatureCollection);

            layout = new Layout(vg);

            var bg = new ImageNode();
            bg.Image = NvgImage.FromFile(vg, "assets/thinfishcy.jpeg");
            bg.Style.FillStrategy = "cover";
            layout.Children.Add(bg);           

            layout.Arrange();
        }

        public void Update()
        {
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            layout.Arrange();
            layout.Update();
            layout.Draw();

            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);

            vg.Translate((Quantity)"70vw", (Quantity)"60vh");
            vg.Scale(1.75f, 1.75f);
            vg.Rotate(2);

            map.Draw(vg);

            vg.EndFrame();

        }
    }
}