using System.Drawing;
using System.Numerics;
using System.Xml.Linq;
using XPlat.Engine.Serialization;
using static TinyC2.TinyC2Api;

namespace XPlat.Engine.Components
{
    public enum ColliderMode
    {
        Ghost = 0,
        Passive = 1,
        Active = 2,
    }

    [SceneElement("collider2d")]
    public class Collider2dComponent : Component
    {
        public c2Shape Shape;
        public c2Shape ShapeTransformed;
        public c2AABB BoundingBox { get; private set; }
        public ColliderMode Mode = ColliderMode.Passive;
        public float Weight { get; set; }

        public void UpdateBoundingBox(ref Matrix4x4 mat)
        {
            BoundingBox = Shape.GetBBox(ref mat);
        }

        public void UpdateTransformedShape(ref Matrix4x4 mat){
            ShapeTransformed = Shape.GetTransformed(ref mat);
        }

        public float Radius {
            set {
                if(Shape == null) return;
                switch(Shape){
                    case c2Circle c:
                        c.r = value;
                        break;
                    case c2Capsule ca:
                        ca.r = value;
                        break;
                    default:
                        break;
                }
            }
            get {
                if(Shape == null) return 0;
                switch(Shape){
                    case c2Circle c:
                        return c.r;
                    case c2Capsule ca:
                        return ca.r;
                    default:
                        return 0;
                }
            }
        }

        public override void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("weight", out var valS) && float.TryParse(valS, out var weight)) Weight = weight;
            if(el.TryGetAttribute("mode", out var val) && Enum.TryParse<ColliderMode>(val, true, out var mode)) Mode = mode;
            Shape = ParseShape(
                el.Elements().FirstOrDefault() ?? throw new InvalidDataException("Collider must have a shape (capsule, polygon, circle or polygon) element"), 
                reader);
            
            base.Parse(el, reader);
        }

        private c2Shape ParseShape(XElement el, SceneReader reader){
            switch(el.Name.ToString()){
                case "circle":
                    var c = Shape is c2Circle ? Shape as c2Circle : new c2Circle();
                    if(el.TryGetAttribute("p", out var valp)) c.p = valp.Vector2();
                    if(el.TryGetAttribute("r", out var valr) && int.TryParse(valr, out var r)) c.r = r;
                    return c;
                case "capsule":
                    var ca = Shape is c2Capsule ? Shape as c2Capsule : new c2Capsule();
                    if(el.TryGetAttribute("a", out var vala)) ca.a = vala.Vector2();
                    if(el.TryGetAttribute("b", out var valb)) ca.b = valb.Vector2();
                    if(el.TryGetAttribute("r", out var valr2) && int.TryParse(valr2, out var r2)) ca.r = r2;
                    return ca;
                case "aabb":
                    var a = Shape is c2AABB ? Shape as c2AABB : new c2AABB();
                    if(el.TryGetAttribute("min", out var min)) a.min = min.Vector2();
                    if(el.TryGetAttribute("max", out var max)) a.max = max.Vector2();
                    return a;
                default: throw new InvalidDataException($"Shape '{el.Name}' is not supported");
            }
        }

        // TODO: Make c2Shape a struct to avoid this
        public override Component Clone(Node n)
        {
            var c = base.Clone(n) as Collider2dComponent;
            c.Shape = Shape.Clone();
            return c;
        }
    }
}

