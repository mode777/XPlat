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
            // TODO
        }
    }
}
