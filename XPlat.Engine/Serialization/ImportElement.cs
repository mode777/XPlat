using System.Xml.Linq;

namespace XPlat.Engine.Serialization
{
    [SceneElement("import")]
    public class ImportElement : ISceneElement
    {
        public void Parse(XElement el, SceneReader reader)
        {
            var assembly = el.Attribute("assembly")?.Value ?? throw new InvalidDataException("Import element must have assembly attribute");
            var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assembly) ?? throw new InvalidDataException($"No assembly named '{assembly}' is currently loaded");
            reader.LoadElementsFromAssembly(asm);
        }
    }
}

