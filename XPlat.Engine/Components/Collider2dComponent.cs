using System.Drawing;
using System.Numerics;
using static TinyC2.TinyC2Api;

namespace XPlat.Engine.Components
{
    public enum ColliderMode {
        Ghost = 0,
        Passive = 1,
        Active = 2,
    }

    [SceneElement("collider2d")]
    public class Collider2dComponent : Component
    {
        public c2Shape Shape;
        public c2AABB BoundingBox { get; private set; }
        public ColliderMode Mode;

        public void UpdateBoundingBox(ref Matrix4x4 mat){
            BoundingBox = Shape.GetBBox(ref mat);
        }
    }
}

