using System.Numerics;

namespace XPlat.Engine
{
    public class CollisionInfo {
     
        public Node Other;
        public Vector2 Normal;
        public float NormalX => Normal.X;
        public float NormalY => Normal.Y;
        public Vector2 Point;
        public float Distance;
    }
}