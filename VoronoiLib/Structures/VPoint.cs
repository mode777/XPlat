namespace VoronoiLib.Structures
{
    public class VPoint : IEquatable<VPoint>
    {
        public double X { get; }
        public double Y { get; }

        internal VPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(VPoint? other)
        {
            if(other == null) return false;
            return X == other.X && Y == other.Y;
        }
    }
}
