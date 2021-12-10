using System.Collections.Generic;
using System.Numerics;

namespace VoronoiLib.Structures
{
    public class FortuneSite
    {
        public double X { get; }
        public double Y { get; }

        public List<VEdge> Cell { get; private set; }
        public HashSet<Vector2> Points { get; init; } = new HashSet<Vector2>();

        public List<FortuneSite> Neighbors { get; private set; }

        public FortuneSite(double x, double y)
        {
            X = x;
            Y = y;
            Cell = new List<VEdge>();
            Neighbors = new List<FortuneSite>();
        }

        public void AddEdge(VEdge edge){
            if(Cell.Count == 0) {
                Cell.Add(edge);
                return;
            }
            int idx = -1;
            for (int i = 0; i < Cell.Count; i++)
            {
                if(Cell[i].End.Equals(edge.Start)){
                    idx = i+1;
                    break;
                }
            }
            if(idx == -1){
                Cell.Add(edge);
                return;
            } 
            Cell.Insert(idx, edge);
        }

        // public override int GetHashCode()
        // {
        //     return X.GetHashCode () ^ Y.GetHashCode ();
        // }
    }
}
