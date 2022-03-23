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
        private readonly IPlatform platform;

        public MapGenSample(IPlatform platform)
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
                   
                    vg.BeginPath();
                    vg.Rect(x, y, 1, 1);
                    var currentNode = mg.Root.LeafAt(x, y);
                    var col = mg.IsOnCriticalPath(currentNode) ? "#ffffff" : "#888888";
                    if (currentNode == mg.StartRoom) col = "#00ff00";
                    if (currentNode == mg.EndRoom) col = "#ff0000";
                    if (elm == LevelElement.Door) col = "#ff0000";
                    if (elm == LevelElement.POI) col = "#ffff00";
                    if (elm == LevelElement.Blocked) col = "#00ffff";
                    if (elm == LevelElement.Hole) col = "#0000ff";

                    vg.FillColor(col);
                    
                    if(elm != LevelElement.Wall)
                        vg.Fill();
                    
                }
            }

            vg.EndFrame();
        }
    }
}