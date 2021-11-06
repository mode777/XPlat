using GLES2;
using NanoVGDotNet;
using System.Drawing;
using net6test.UI;
using net6test.MapGenerator;

namespace net6test.samples
{
    public class MapGenSample : ISdlApp
    {
        private NVGcontext vg;
        private MapGen mg;
        private Random ran;
        private readonly IPlatformInfo platform;

        public MapGenSample(IPlatformInfo platform)
        {
            this.platform = platform;

        }

        public void Init()
        {
            vg = new NVGcontext();
            GlNanoVG.nvgCreateGL(ref vg, (int)NVGcreateFlags.NVG_ANTIALIAS |
                        (int)NVGcreateFlags.NVG_STENCIL_STROKES);
            var pg = new ProdGen(1986);
            mg = new MapGen(pg, 64, 64, 10);
            ran = new Random();
            // for (int y = 0; y < mg.Map.H; y++)
            // {
            //     for (int x = 0; x < mg.Map.W; x++)
            //     {
            //         var v = mg.Map[x,y];
            //         if(v == LevelElement.Wall) Console.Write("X");
            //         if(v == LevelElement.Floor) Console.Write("0");
            //     }
            //     Console.Write("\n");
            // }
        }

        public void Update()
        {
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);
            vg.Scale(8,8);
            var cols = new List<NVGcolor> {
                vg.RGBA(255,0,0,255),
                vg.RGBA(255,255,0,255),
                vg.RGBA(255,255,255,255),
                vg.RGBA(0,0,0,255),
                vg.RGBA(0,0,255,255),
                vg.RGBA(255,0,255,255),
                vg.RGBA(0, 255, 255, 255),
                vg.RGBA(255, 0, 255, 255),
                vg.RGBA(128, 0, 128, 255),
                vg.RGBA(128,0,0,255),
                vg.RGBA(128,128,0,255),
                vg.RGBA(128,128,128,255),
                vg.RGBA(0,0,128,255),
                vg.RGBA(128,0,128,255),
                vg.RGBA(0, 128, 128, 255),
            };
            var i = 0;

            foreach (var r in mg.Rooms)
            {
                var q = r.Quad;
                vg.BeginPath();
                
                var col = cols[i%cols.Count];
                vg.FillColor(col);

                vg.Rect(q.X, q.Y, q.Width, q.Height);
                vg.Fill();
                i++;
            }

            vg.EndFrame();
        }
    }
}