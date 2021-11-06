using System.Drawing;

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
        public RoomGen(ProdGen prodGen, LevelMap map){
            ProdGen = prodGen;
            Map = map;
        }

        public ProdGen ProdGen { get; }
        public LevelMap Map { get; }

        public void Generate(Node n){
            foreach (var c in n.Connections)
            {
                Map[c.Pos.X, c.Pos.Y] = c.A.IsEnabled && c.B.IsEnabled && !c.IsDisabled ? LevelElement.Door : LevelElement.Wall;
            }
            if (n.IsEnabled)
            {
                Map.Fill(n.Quad.X+1, n.Quad.Y+1, n.Quad.Width-1, n.Quad.Height-1, (x, y) => LevelElement.Floor);
            }
            //Populate(n);
        }

        private void Populate(Node n)
        {
            var targets = new List<int>();
            var path = new List<int>();
            var ox = n.Quad.X + 1;
            var oy = n.Quad.Y + 1;
            var grid = Map.SubGrid(ox, oy, n.Quad.Width-1, n.Quad.Height-1);
            var hFirst = ProdGen.Roll(50);

            foreach (var c in n.Connections)
            {
                if(c.Pos.X == n.Quad.Y) targets.Add(grid.Id(c.Pos.X+1,c.Pos.Y-oy));
                if(c.Pos.Y == n.Quad.Y) targets.Add(grid.Id(c.Pos.X-ox, c.Pos.Y + 1 - oy));
                if(c.Pos.Y == n.Quad.Y+n.Quad.Height) targets.Add(grid.Id(c.Pos.X-ox, c.Pos.Y + 1 - oy));
                if(c.Pos.X == n.Quad.X+n.Quad.Width) targets.Add(grid.Id(c.Pos.X-1-ox, c.Pos.Y - oy));
            }

            if(targets.Count == 0) return;

            var first = targets[0];
            foreach (var t in targets.Skip(1))
            {
                var target = grid.Coords(t);
                var current = grid.Coords(first);
                
                var c1 = current.X == 0 || current.X == grid.W-1 ? 0 : 1;
                var c2 = c1 == 1 ? 0 : 1;
                var sel = (Point p, int idx) => idx == 0 ? p.X : p.Y;
                var asg = (ref Point p, int idx, int val) => { if(idx == 0){ p.X=val; } else { p.Y=val; } };
                var dirC1 = sel(target,c1) > sel(current,c1) ? 1 : -1;
                var dirC2 = sel(target, c2) > sel(current,c2) ? 1 : -1;

                while(sel(current,c1) != sel(target,c1)){
                    asg(ref current, c1, sel(current,c1)+dirC1);
                    path.Add(grid.Id(current.X, current.Y));
                }
                while(sel(current,c2) != sel(target,c2)){
                    asg(ref current, c2, sel(current,c2)+dirC2);
                    path.Add(grid.Id(current.X, current.Y));
                }

                foreach (var id in path)
                    grid[id] = LevelElement.Floor;
                
                foreach (var id in targets)
                {
                    var co = grid.Coords(id);
                    var w = ProdGen.Size(grid.W, grid.W/2);
                    var h = ProdGen.Size(grid.H, grid.H/2);
                    grid.Fill(co.X-(w/2),co.Y-h/2,w,h, (int ox, int oy) => LevelElement.Floor);
                    grid[id] = LevelElement.Floor;
                }
                Map.PutGrid(grid, ox, oy);
            }
        }


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