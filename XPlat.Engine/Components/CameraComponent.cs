using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.Graphics;

namespace XPlat.Engine.Components
{

    [SceneElement("camera")]
    public class CameraComponent : Component {

        public CameraComponent(){
            Camera = new Camera3d();
        }
        public Camera3d Camera { get; set; }
        public Node? Node { get; set; }

        public override void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("farplane", out var raw) && float.TryParse(raw, out var farplane)) Camera.FarPlane = farplane;
            if(el.TryGetAttribute("nearplane", out var raw2) && float.TryParse(raw2, out var nearplane)) Camera.NearPlane = nearplane;
            if(el.TryGetAttribute("fov", out var raw3) && float.TryParse(raw3, out var fov)) Camera.Fov = fov;
        }
    }
}
