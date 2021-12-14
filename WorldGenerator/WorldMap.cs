using System;
using System.Numerics;
using NanoVGDotNet;
using SimplexNoise;
using VoronoiLib;
using VoronoiLib.Structures;

namespace net6test.WorldGenerator
{ 
    public class WorldCell 
    {
        static double Ccw(IPoint a, IPoint b, IPoint c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y);
        }

        public FortuneSite Site { get; set; }
        public List<VPoint> Points { get; set; }
        public NVGcolor Color { get; set; }
        public float PerlinValue { get; set; }

        public bool isComplete => Points.Count > 3 && Points[0].Equals(Points.Last());
        public int Winding => isComplete ? Math.Sign(Ccw(Site, Points[0], Points[1])) : 0;
    }

    public class WorldMapParams
    {
        public Vector2 Size { get; set; }
        public float PointsJitter { get; set; } = 0.5f;
        public Vector2 GridSize { get; set; } = new Vector2(25);
        public int Seed { get; set; } = DateTime.Now.Millisecond;
        public float DrawingScale { get; set; } = 1;
        public float WaterThreshold { get; set; } = 0.5f;
    }

    public class WorldMap
    {
        private readonly WorldMapParams param;
        private readonly Random r;
        private LinkedList<VEdge> edges;
        private List<WorldCell> cells;
        private List<FortuneSite> points;

        public WorldMap(WorldMapParams param)
        {
            this.param = param;
            this.r = new Random(param.Seed);
        }

        public void Generate(){
            Noise.Seed = param.Seed;
            GeneratePoints();
            GenerateVoroni();
        }

        private void GeneratePoints(){
            this.points = new List<FortuneSite>();
            for (float y = 0; y < param.Size.Y; y+=param.GridSize.Y)
            {
                for (float x = 0; x < param.Size.X; x+=param.GridSize.X)
                {
                    points.Add(new FortuneSite( x + param.PointsJitter * param.GridSize.X * (r.NextSingle() - r.NextSingle()), 
                                            y + param.PointsJitter * param.GridSize.Y * (r.NextSingle() - r.NextSingle())));
                }
            }
        }

        private List<VPoint> ConnectEdges(IEnumerable<VEdge> edges)
        {
            var pool = new HashSet<VEdge>(edges);
            var l = new List<VPoint>();

            if (pool.Count < 3) return l;
            
            var startEdge = pool.First();
            l.Add(startEdge.Start);
            l.Add(startEdge.End);
            pool.Remove(startEdge);

            var head = l[0];
            var tail = l[1];
            
            while (!tail.Equals(head))
            {
                var nextStart = pool.FirstOrDefault(x => x.Start.Equals(tail));
                if (nextStart != null)
                {
                    tail = nextStart.End;
                    l.Add(tail);
                    pool.Remove(nextStart);
                    continue;
                }
                var nextEnd = pool.FirstOrDefault(x => x.End.Equals(tail));
                if (nextEnd != null)
                {
                    tail = nextEnd.Start;
                    l.Add(tail);
                    pool.Remove(nextEnd);
                    continue;
                }
                break;
            }

            return l;
        }

        private float CalculateNoise(IPoint point)
        {
            float n2d(float s) => Noise.CalcPixel2D((int)point.X, (int)point.Y, s) / 255;

            var val = n2d(0.001f) * 0.5f + n2d(0.002f) * 0.2f + n2d(0.005f) * 0.15f + n2d(0.02f) * 0.05f;

            return val;
        }

        private void GenerateVoroni(){
            this.edges = FortunesAlgorithm.Run(points, 0, 0, param.Size.X, param.Size.Y);
            foreach (var edge in edges)
            {
                edge.Left.Cell.Add(edge);
                edge.Right.Cell.Add(edge);
            }

            this.cells = new List<WorldCell>();

            foreach (var point in points)
            {
                var cell = new WorldCell
                {
                    Site = point,
                    Points = ConnectEdges(point.Cell),
                    PerlinValue = CalculateNoise(point),
                    Color = NanoVG.nvgRGBA((byte)r.Next(255),(byte)r.Next(255),(byte)r.Next(255), 255)
                };
                cell.Color = cell.PerlinValue > param.WaterThreshold ? new NVGcolor { r = cell.PerlinValue, g = cell.PerlinValue, b = cell.PerlinValue, a = 1 } : "#0000ff";

                if (cell.Winding == -1) cell.Points.Reverse();
                cells.Add(cell);
            }
            cells = cells.Where(x => x.isComplete).ToList();
        }

        public void Draw(NVGcontext vg, float cellIdx){

            var s = param.DrawingScale;
            foreach (var c in cells)
            {
                vg.BeginPath();
                vg.MoveTo((float)c.Points[0].X * s, (float)c.Points[0].Y * s);

                for (int i = 1; i < c.Points.Count - 1; i++)
                {
                    vg.LineTo((float)c.Points[i].X * s, (float)c.Points[i].Y * s);
                }

                vg.ClosePath();
                vg.FillColor(c.Color);
                vg.Fill();
            }

            //foreach (var p in points)
            //{
            //    vg.BeginPath();
            //    vg.Rect((float)p.X * s, (float)p.Y * s, 3 * s, 3 * s);
            //    vg.FillColor("#ff0000");
            //    vg.Fill();
            //}
            

            ////foreach(var c in cells)
            ////{
            //var cell = cells[(int)cellIdx];
            ////    //if(!c.isComplete) return;



            //for (int i = 0; i < cell.Points.Count - 1; i++)
            //{
            //    vg.FontSize(s * 20);
            //    vg.FontFace("sans");
            //    vg.FillColor("#000000");
            //    vg.Text((float)cell.Points[i].X * s, (float)cell.Points[i].Y * s, i.ToString());
            //    //vg.Rect((float)c.Points[i].X*s, (float)c.Points[i].Y*s, 4*s,4*s);
            //}
            //vg.FillColor(c.Color);
            //vg.Fill();
            //}

            // foreach (var p in points)
            // {
            //     vg.BeginPath();
            //     vg.MoveTo((float)edge.Start.X * s, (float)edge.Start.Y * s);
            //     vg.LineTo((float)edge.End.X * s, (float)edge.End.Y * s);
            //     vg.Stroke();
            // }
            // vg.BeginPath();
            // foreach (var p in points)
            // {
            //     vg.Circle((float)p.X * param.DrawingScale, (float)p.Y * param.DrawingScale, param.DrawingScale * 2);
            // }
            // vg.FillColor("#ff0000");
            // vg.Fill();
        }
    }
}