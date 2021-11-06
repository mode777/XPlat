using System.Drawing;

namespace net6test.MapGenerator
{
    public class Connection
    {
        public Connection(Node a, Node b, Point pos)
        {
            A = a;
            B = b;
            Pos = pos;
        }

        public Node A { get; }
        public Node B { get; }
        public Point Pos { get; }
        public bool IsDisabled { get; set; } = false;
    }


}