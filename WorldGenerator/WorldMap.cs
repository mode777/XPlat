using System;
using System.Numerics;
using NanoVGDotNet;
using VoronoiLib;
using VoronoiLib.Structures;

namespace net6test.WorldGenerator
{
    class ConvexHull
    {
        public static double cross(VPoint O, VPoint A, VPoint B)
        {
            return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
        }

        public static List<VPoint> GetConvexHull(List<VPoint> points)
        {
            if (points == null)
                return null;

            if (points.Count() <= 1)
                return points;

            int n = points.Count(), k = 0;
            List<VPoint> H = new List<VPoint>(new VPoint[2 * n]);

            points.Sort((a, b) =>
                a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));

            // Build lower hull
            for (int i = 0; i < n; ++i)
            {
                while (k >= 2 && cross(H[k - 2], H[k - 1], points[i]) <= 0)
                    k--;
                H[k++] = points[i];
            }

            // Build upper hull
            for (int i = n - 2, t = k + 1; i >= 0; i--)
            {
                while (k >= t && cross(H[k - 2], H[k - 1], points[i]) <= 0)
                    k--;
                H[k++] = points[i];
            }

            return H.Take(k - 1).ToList();
        }
    }

    public class WorldCell 
    {
        public FortuneSite Site { get; set; }
        public List<VPoint> Points { get; set; }
        public NVGcolor Color { get; set; }

        public bool isComplete => Points[0].Equals(Points.Last()) && Points.Count > 3;
        public int Dot => (int)MathF.Sign(Vector2.Dot(
            new Vector2((float)Points[0].X - (float)Site.X, (float)Points[0].Y - (float)Site.Y),
            new Vector2((float)Points[1].X - (float)Site.X, (float)Points[1].Y - (float)Site.Y)
          ));
    }

    public class WorldMapParams
    {
        public Vector2 Size { get; set; }
        public float PointsJitter { get; set; } = 0.5f;
        public Vector2 GridSize { get; set; } = new Vector2(25);
        public int Seed { get; set; } = DateTime.Now.Millisecond;
        public float DrawingScale { get; set; } = 1;
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
                var c = point.Cell;
                var l = new List<VPoint>();
                l.Add(c[0].Start);
                l.Add(c[0].End);
                c.Remove(c[0]);
                var head = l[0];
                var tail = l[1];
                while(!tail.Equals(head)){
                    var nextStart = c.FirstOrDefault(x => x.Start.Equals(tail));
                    if(nextStart != null){
                        tail = nextStart.End;
                        l.Add(tail);
                        c.Remove(nextStart);
                        continue;
                    }
                    var nextEnd = c.FirstOrDefault(x => x.End.Equals(tail));
                    if(nextEnd != null){
                        tail = nextEnd.Start;
                        l.Add(tail);
                        c.Remove(nextEnd);
                        continue;
                    }
                    break;
                }
                var cell = new WorldCell
                {
                    Site = point,
                    Points = l,
                    Color = NanoVG.nvgRGBA((byte)r.Next(255),(byte)r.Next(255),(byte)r.Next(255), 255)
                };
                if(cell.Dot == -1) cell.Points.Reverse();
                cells.Add(cell);
            }
            cells = cells.Where(x => x.isComplete).ToList();
        }

        public void Draw(NVGcontext vg, float cellIdx){

            var s = param.DrawingScale;
            foreach (var c in cells)
            {
                if (!c.isComplete) continue;
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

            //vg.StrokeColor("#00ff00");
            //vg.StrokeWidth(s);
            //foreach (var e in edges)
            //{
            //    vg.BeginPath();
            //    vg.MoveTo((float)e.Start.X*s, (float)e.Start.Y*s);
            //    vg.LineTo((float)e.End.X*s, (float)e.End.Y*s);
            //    vg.ClosePath();
            //    vg.StrokeColor("#00ff00");
            //    vg.Stroke();
            //}
            // foreach (var c in cells)
            // {
            //     if(!c.isComplete) continue;
            //     //if(c.Dot == -1) continue;
            //     vg.BeginPath();
            //     vg.MoveTo((float)c.Points[0].X*s, (float)c.Points[1].Y*s);
            //     for (int i = 1; i < c.Points.Count; i++)
            //     {
            //         vg.LineTo((float)c.Points[i].X*s, (float)c.Points[i].Y*s);
            //     }
            //     vg.ClosePath();
            //     vg.StrokeColor(c.Color);
            //     vg.Stroke();
            //     // vg.FillColor(c.Color);
            //     // vg.Fill();

            // }

            

            //foreach(var c in cells)
            //{
            //    var c = cells[(int)cellIdx];
            //    //if(!c.isComplete) return;



            //for (int i = 0; i < c.Points.Count-1; i++)
            //    {
            //        vg.FontSize(s*20);
            //        vg.FontFace("sans");
            //        vg.FillColor(c.Color);
            //        vg.Text((float)c.Points[i].X*s, (float)c.Points[i].Y*s, i.ToString());
            //        //vg.Rect((float)c.Points[i].X*s, (float)c.Points[i].Y*s, 4*s,4*s);
            //    }
            //    //vg.FillColor(c.Color);
            //    //vg.Fill();
            ////}

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