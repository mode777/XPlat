using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.Graphics;

namespace XPlat.Engine.Components
{
    [SceneElement("camera2d")]
    public class Camera2dComponent : Component {

        public Camera2dComponent(){
            Camera = new Camera2d();
        }
        public Camera2d Camera { get; set; }
        public Node? Node { get; set; }

        public override void Parse(XElement el, SceneReader reader)
        {
            // TODO
        }
    }
}
