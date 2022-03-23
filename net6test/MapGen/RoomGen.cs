using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace net6test.MapGenerator
{
    // public class Room
    // {
    //     public Room(Node node)
    //     {
    //         Node = node;
    //     }

    //     public Node Node { get; }
    // }

    public class RoomGen {
        public RoomGen(ProdGen prodGen, int w, int h){
            ProdGen = prodGen;
            Map = new LevelMap(w,h);
        }

        public ProdGen ProdGen { get; }
        public LevelMap Map { get; }

        public void Generate(Node n){
            Map.Fill(n.Quad.X + 1, n.Quad.Y + 1, n.Quad.Width - 1, n.Quad.Height - 1, (x, y) => LevelElement.Floor);
            foreach (var c in n.ActiveConnections)
            {
                Map[c.Pos.X, c.Pos.Y] = LevelElement.Door;
                Map.Fill(c.Pos.X - 1, c.Pos.Y - 1, 3, 3, (x, y, v) => v == LevelElement.Floor ? LevelElement.Blocked : v);                
            }
            Populate(n);
            MarkPois(n);
            //Populate(n);
        }

        private void MarkPois(Node n)
        {
            Map.ForEachXY(n.Quad.X, n.Quad.Y, n.Quad.Width, n.Quad.Height, (x, y, v) =>
            {
                if(Map[x,y] == LevelElement.Floor && Map.Neighbours(x, y).All(x => x == LevelElement.Floor))
                {
                    Map[x, y] = LevelElement.POI;
                    n.POIs.Add(new Point(x, y));
                }
            });
        }

        private void Populate(Node n)
        {
            var grid = Map.SubGrid(n.Quad.X + 1, n.Quad.Y + 1, n.Quad.Width - 1, n.Quad.Height - 1);

            CreateLs(grid, ProdGen.Roll(80) ? LevelElement.Wall : LevelElement.Hole);
            CreateColumns(grid);

            Map.PutGrid(grid, n.Quad.X + 1, n.Quad.Y + 1);
        }

        private void CreateColumns(Grid<LevelElement> grid)
        {
            for (int y = 0; y < grid.H; y++)
            {
                for (int x = 0; x < grid.W; x++)
                {
                    var v = grid[x, y];
                    if (grid[x, y] == LevelElement.Floor && ProdGen.Roll(15))
                    {
                        if (grid.Neighbours(x, y).All(x => x == LevelElement.Floor) &&
                                grid.Neighbours(x + 1, y).All(x => x == LevelElement.Floor) &&
                                grid.Neighbours(x + 1, y +1).All(x => x == LevelElement.Floor) &&
                                grid.Neighbours(x, y + 1).All(x => x == LevelElement.Floor))
                        {
                            var el = ProdGen.Roll(50) ? LevelElement.Wall : LevelElement.Hole;
                            grid[x, y] = el;
                            grid[x + 1, y] = el;
                            grid[x + 1, y+1] = el;
                            grid[x, y+1] = el;
                        }

                       
                    }
                }
            }           
        }

        private void CreateLs(Grid<LevelElement> grid, LevelElement el)
        {
            var w = ProdGen.Size(grid.W, 2);
            var h = ProdGen.Size(grid.H, 2);

            var l = new List<Rectangle> {
                new Rectangle(0, 0, w, h),
                new Rectangle(w, 0, grid.W-w, h),
                new Rectangle(0, h, w, grid.H-h),
                new Rectangle(w, h, grid.W-w, grid.H-h),
            };
            ProdGen.Shuffle(l);

            l.FirstOrDefault(l => FillIfEmpty(grid, l, el));
        }

        private bool FillIfEmpty(Grid<LevelElement> grid, Rectangle r, LevelElement el)
        {
            var possible = grid.SubGrid(r.X, r.Y, r.Width, r.Height).All(x => x == LevelElement.Floor);
            if (possible)
            {
                grid.Fill(r.X, r.Y, r.Width, r.Height, (x, y) => el);
            }
            return possible;
        }

        //private void Populate(Node n)
        //{
        //    var targets = new List<int>();
        //    var path = new List<int>();
        //    var ox = n.Quad.X + 1;
        //    var oy = n.Quad.Y + 1;
        //    var grid = Map.SubGrid(ox, oy, n.Quad.Width-1, n.Quad.Height-1);
        //    var hFirst = ProdGen.Roll(50);

        //    foreach (var c in n.Connections)
        //    {
        //        if(c.Pos.X == n.Quad.Y) targets.Add(grid.Id(c.Pos.X+1,c.Pos.Y-oy));
        //        if(c.Pos.Y == n.Quad.Y) targets.Add(grid.Id(c.Pos.X-ox, c.Pos.Y + 1 - oy));
        //        if(c.Pos.Y == n.Quad.Y+n.Quad.Height) targets.Add(grid.Id(c.Pos.X-ox, c.Pos.Y + 1 - oy));
        //        if(c.Pos.X == n.Quad.X+n.Quad.Width) targets.Add(grid.Id(c.Pos.X-1-ox, c.Pos.Y - oy));
        //    }

        //    if(targets.Count == 0) return;

        //    var first = targets[0];
        //    foreach (var t in targets.Skip(1))
        //    {
        //        var target = grid.Coords(t);
        //        var current = grid.Coords(first);

        //        var c1 = current.X == 0 || current.X == grid.W-1 ? 0 : 1;
        //        var c2 = c1 == 1 ? 0 : 1;
        //        var sel = (Point p, int idx) => idx == 0 ? p.X : p.Y;
        //        var asg = (ref Point p, int idx, int val) => { if(idx == 0){ p.X=val; } else { p.Y=val; } };
        //        var dirC1 = sel(target,c1) > sel(current,c1) ? 1 : -1;
        //        var dirC2 = sel(target, c2) > sel(current,c2) ? 1 : -1;

        //        while(sel(current,c1) != sel(target,c1)){
        //            asg(ref current, c1, sel(current,c1)+dirC1);
        //            path.Add(grid.Id(current.X, current.Y));
        //        }
        //        while(sel(current,c2) != sel(target,c2)){
        //            asg(ref current, c2, sel(current,c2)+dirC2);
        //            path.Add(grid.Id(current.X, current.Y));
        //        }

        //        foreach (var id in path)
        //            grid[id] = LevelElement.Floor;

        //        foreach (var id in targets)
        //        {
        //            var co = grid.Coords(id);
        //            var w = ProdGen.Size(grid.W, grid.W/2);
        //            var h = ProdGen.Size(grid.H, grid.H/2);
        //            grid.Fill(co.X-(w/2),co.Y-h/2,w,h, (int ox, int oy) => LevelElement.Floor);
        //            grid[id] = LevelElement.Floor;
        //        }
        //        Map.PutGrid(grid, ox, oy);
        //    }
        //}


        private bool HasNeighbour(int x, int y, LevelElement e){
            foreach (var n in Map.Neighbours(x,y))
            {
                if(n == e){
                    return true;
                }
            }
            return false;
        }

        private bool IsFree(int x, int y, int w, int h){
            var f = LevelElement.Floor;
            for (int cy = y; cy < y+h; cy++)
            {
                for (int cx = x; cx < x+w; cx++)
                {
                    if(Map[cx,cy] != f) return false;
                }
            }
            return true;
        }



    }


}