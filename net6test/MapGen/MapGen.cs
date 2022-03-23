using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace net6test.MapGenerator
{
    public enum LevelElement {
        Floor,
        Wall,
        Door,
        POI,
        Blocked,
        Hole
    }

    public class MapGen
    {
        public MapGen(ProdGen prodGen, int w, int h, int threshold)
        {
            ProdGen = prodGen;
            W = w;
            H = h;
            Threshold = threshold;
            this.Root = new Node(new Rectangle(0,0,w,h));
            Generate();
        }

        public ProdGen ProdGen { get; }
        public int W { get; }
        public int H { get; }
        public int Threshold { get; }
        public Node Root { get; }
        public LevelMap Map { get; private set; }
        public List<Node> Rooms { get; private set; } = new List<Node>();
        public IEnumerable<Node> ActiveRooms => Rooms.Where(x => x.IsEnabled); 
        public Node StartRoom { get; private set; }
        public Node EndRoom { get; private set; }
        public HashSet<Node> CriticalPath { get; private set; }

        public void Generate(){
            Split(Root);
            Rooms = Root.GetLeaves().ToList();
            Rooms.ForEach(r =>
            {
                r.CollectNeighbours();
                Connect(r);
            });
            this.StartRoom = Root.LeafAt(0, 0);
            this.EndRoom = Root.LeafAt(W - 1, H - 1);

            CriticalPath = new HashSet<Node>(Algo.Dijkstra(Rooms, StartRoom, EndRoom));
            //this.EndRoom = ProdGen.Select(Rooms.Where)
            foreach (var r in CriticalPath)
            {
                r.IsEnabled = true;
                var sideroom = r.Neighbours.FirstOrDefault(x => !IsOnCriticalPath(x));
                if (sideroom != null)
                {
                    sideroom.IsEnabled = true;
                    foreach (var c in sideroom.Connections.ToArray())
                    {
                        if (c.A != r && c.B != r)
                        {
                            c.IsDisabled = true;
                        }
                    }
                }
            }

            GenerateRooms();
        }

        public bool IsOnCriticalPath(Node n) => CriticalPath.Contains(n);


        public void Connect(Node n)
        {
            // var connLeft = new List<Point>();
            // var connUp = new List<Point>();
            foreach (var nl in n.NeighboursLeft)
            {
                var start = Math.Max(nl.Quad.Y + 1, n.Quad.Y + 1);
                var end = Math.Min(nl.Quad.Y + nl.Quad.Height, n.Quad.Y + n.Quad.Height);
                var range = end - start - 1;
                if (range <= 1)
                    continue;
                var coords = new Point(n.Quad.X, start + ProdGen.Size(range, 1) + 1);
                var conn = new Connection(n, nl, coords);
                n.AddConnection(conn);
                nl.AddConnection(conn);
                //connLeft.Add(coords);
            }
            foreach (var nu in n.NeighboursUp)
            {
                var start = Math.Max(nu.Quad.X, n.Quad.X);
                var end = Math.Min(nu.Quad.X + nu.Quad.Width, n.Quad.X + n.Quad.Width);
                var range = end - start - 1;
                if (range <= 1)
                    continue;
                var coords = new Point(start + ProdGen.Size(range, 1) + 1, n.Quad.Y);
                var conn = new Connection(n, nu, coords);
                n.AddConnection(conn);
                nu.AddConnection(conn);
            }
        }


        private void GenerateRooms()
        {
            var roomGen = new RoomGen(ProdGen, W, H);
            foreach (var r in ActiveRooms)
            {
                roomGen.Generate(r);
            }
            Map = roomGen.Map;
        }

        private void Split(Node n){
            if(n.Quad.Width * n.Quad.Height > (Threshold * Threshold * 5)){
                n.Split(ProdGen, Threshold);
                Split(n.A);
                Split(n.B);
            }
        }

    }


}