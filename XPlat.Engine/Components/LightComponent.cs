using System.Numerics;
using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.Graphics;

namespace XPlat.Engine.Components
{
    [SceneElement("light")]
    public class LightComponent : Component {

        public PointLight Light { get; set; } = new PointLight();
        public Node? Node { get; set; }

        public override void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("color", out var color)) Light.Color = color.Vector3();
            if(el.TryGetAttribute("intensity", out var raw) && float.TryParse(raw, out var intensity)) Light.Intensity = intensity;
        }
    }
}
