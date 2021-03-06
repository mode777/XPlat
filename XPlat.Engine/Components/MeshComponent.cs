using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.Graphics;

namespace XPlat.Engine.Components
{
    [SceneElement("mesh")]
    public class MeshComponent : Component {
        public Mesh Mesh { get; set; }
        public Node? Node { get; set; }

        public override void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("src", out var src)) { 
                var split = src.Split(':');
                Mesh = reader.LoadGltfNode(split[0], split[1])?.ReadMesh();
            }

            if(el.TryGetAttribute("res", out var res)) { 
                var resource = reader.Resources.Load(res);
                Mesh = resource.GetValue<Mesh>();
                resource.Changed += (s,a) => Mesh = resource.GetValue<Mesh>();
            }
        }
    }
}
