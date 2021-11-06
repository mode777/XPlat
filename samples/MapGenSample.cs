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
            var pg = new ProdGen(851);
            mg = new MapGen(pg, 64, 64, 6);
            //for (int y = 0; y < mg.Map.H; y++)
            //{
            //    for (int x = 0; x < mg.Map.W; x++)
            //    {
            //        var v = mg.Map[x, y];
            //        if (v == LevelElement.Wall) Console.Write("X");
            //        if (v == LevelElement.Floor) Console.Write("0");
            //    }
            //    Console.Write("\n");
            //}
        }

        public void Update()
        {
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);
            vg.Scale(8,8);           
            var i = 0;

            //foreach (var r in mg.Rooms)
            //{
            //    var q = r.Quad;
            //    vg.BeginPath();

            //    vg.StrokeColor("#00ff00");

            //    vg.Rect(q.X, q.Y, q.Width, q.Height);
            //    vg.Stroke();
            //    i++;
            //}

            for (int y = 0; y < mg.Map.H; y++)
            {
                for (int x = 0; x < mg.Map.W; x++)
                {
                    var elm = mg.Map[x, y];
                    if (elm == LevelElement.Floor || elm == LevelElement.Door)
                    {
                        vg.BeginPath();
                        vg.Rect(x, y, 1, 1);
                        var currentNode = mg.Root.LeafAt(x, y);
                        var floorCol = mg.IsOnCriticalPath(currentNode) ? "#ffffff" : "#888888";
                        if (currentNode == mg.StartRoom) floorCol = "#00ff00";
                        if (currentNode == mg.EndRoom) floorCol = "#ff0000";
                        vg.FillColor(elm == LevelElement.Floor ? floorCol : "#ff0000");
                        vg.Fill();
                    }
                }
            }

            vg.EndFrame();
        }
    }
}