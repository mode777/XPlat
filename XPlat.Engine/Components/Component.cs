using System.Xml.Linq;
using XPlat.Engine.Serialization;

namespace XPlat.Engine.Components
{
    public abstract class Component : ISceneElement
    {
        public Node? Node { get; set; }

        public virtual void Parse(XElement el, SceneReader reader) {
            // TODO: Implement default parser
        }
    }
}

