using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace net6test.MapGenerator
{
    public class Node
    {
        public bool IsLeaf => A == null && B == null;
        public Node A { get; private set; }
        public Node B { get; private set; }
        public SplitDir Dir { get; private set; }
        public Rectangle Quad { get; }
        public Node Root { get; }
        public List<Connection> Connections { get; private set; } = new List<Connection>();
        public IEnumerable<Connection> ActiveConnections => Connections.Where(c => c.A.IsEnabled && c.B.IsEnabled && !c.IsDisabled);
        public IEnumerable<Node> Neighbours => Connections.Select(x => x.B == this ? x.A : x.B);
        //public IEnumerable<Node> Neighbours => Enumerable.Concat(Enumerable.Concat(NeighboursLeft, NeighboursRight), Enumerable.Concat(NeighboursUp, NeighboursDown));
        public List<Node> NeighboursUp { get; private set; } = new List<Node>();
        public List<Node> NeighboursDown { get; private set; } = new List<Node>();
        public List<Node> NeighboursLeft { get; private set; } = new List<Node>();
        public List<Node> NeighboursRight { get; private set; } = new List<Node>();
        public List<Point> POIs { get; } = new List<Point>();
        public bool IsEnabled { get; set; } = false;


        public Node(Rectangle box)
        {
            Quad = box;
            Root = this;
        }

        public Node(Rectangle box, Node root)
        {
            Quad = box;
            Root = root;
        }

        public void Split(ProdGen pg, int threshold)
        {
            if (Quad.Width >= Quad.Height)
            {
                this.A = new Node(new Rectangle(Quad.X, Quad.Y, pg.Size(Quad.Width, threshold), Quad.Height), Root);
                this.B = new Node(new Rectangle(Quad.X + A.Quad.Width, Quad.Y, Quad.Width - A.Quad.Width, Quad.Height), Root);
                this.Dir = SplitDir.H;
            }
            else
            {
                this.A = new Node(new Rectangle(Quad.X, Quad.Y, Quad.Width, pg.Size(Quad.Height, threshold)), Root);
                this.B = new Node(new Rectangle(Quad.X, Quad.Y + A.Quad.Height, Quad.Width, Quad.Height - A.Quad.Height), Root);
                this.Dir = SplitDir.V;
            }
        }

        public void AddConnection(Connection c) => Connections.Add(c);
        public void RemoveConnection(Connection c) => Connections.Remove(c);

        public Node LeafAt(int x, int y){
            if(!IsInside(x,y)) 
                return null;
            if(IsLeaf) return this;

            if(Dir == SplitDir.H){
                if(x < B.Quad.X) { return A.LeafAt(x,y); }
                else return B.LeafAt(x,y);
            } else {
                if(y < B.Quad.Y) { return A.LeafAt(x,y); }
                else return B.LeafAt(x,y);
            }
        }

        List<Node> CollectDown(int cx, int cy){
            var coll = new List<Node>();
            while(cy < (this.Quad.Y+this.Quad.Height)){
                var n = Root.LeafAt(cx, cy);
                if(n == null) 
                    break;
                cy = n.Quad.Y + n.Quad.Height;
                coll.Add(n);
            }
            return coll;
        }

        List<Node> CollectRight(int cx, int cy){
            var coll = new List<Node>();
            while(cx < (this.Quad.X+this.Quad.Width)){
                var n = Root.LeafAt(cx, cy);
                if(n == null) 
                    break;
                cx = n.Quad.X + n.Quad.Width;
                coll.Add(n);
            }
            return coll;
        }

        public void CollectNeighbours(){
            NeighboursLeft = CollectDown(Quad.X-1, Quad.Y);
            NeighboursRight = CollectDown(Quad.X+Quad.Width, Quad.Y);
            NeighboursUp = CollectRight(Quad.X, Quad.Y-1);
            NeighboursDown = CollectRight(Quad.X, Quad.Y+Quad.Height);
        }

        public bool IsInside(int x, int y) => x >= this.Quad.X && x < (this.Quad.X + this.Quad.Width) && y >= this.Quad.Y && y < (this.Quad.Y + this.Quad.Height);

        public Point Center(){
            return new Point(Quad.X+Quad.Width/2, Quad.Y+Quad.Height/2);
        }

        public IEnumerable<Node> GetLeaves(){
            if(IsLeaf) return new Node[] { this };
            return Enumerable.Concat(A.GetLeaves(),B.GetLeaves()); 
        }
    }


}