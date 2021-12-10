using GLES2;
using NanoVGDotNet;
using System.Drawing;
using net6test.UI;
using System.Numerics;

namespace net6test.WorldGenerator
{
    public class WorldGenApp : ISdlApp
    {
        private NVGcontext vg;
        private WorldMap map;
        private readonly IPlatformInfo platform;

        public WorldGenApp(IPlatformInfo platform)
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

            this.map = new WorldMap(new WorldMapParams{
                Size = new Vector2(platform.WindowSize.Width, platform.WindowSize.Height),
                DrawingScale = platform.RetinaScale,
                PointsJitter = 0.5f,
                GridSize = new Vector2(12)
            });
            this.map.Generate();

        }

        public void Update()
        {
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);
            //vg.Rotate(Time.RunningTime);
            this.map.Draw(vg, Time.RunningTime/2);

            vg.EndFrame();
        }
    }
}