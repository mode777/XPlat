using System.Xml.Linq;

namespace XPlat.Engine.Serialization
{
    public interface ISceneElement {
        void Parse(XElement el, SceneReader reader);
    }
}

