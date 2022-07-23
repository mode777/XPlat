using System.Reflection;
using System.Xml.Linq;

namespace XPlat.Engine.Serialization
{
    // Deprecated
    [SceneElement("import")]
    public class ImportElement : ISceneElement
    {
        public void Parse(XElement el, SceneReader reader)
        {
            var assembly = el.Attribute("assembly")?.Value ?? throw new InvalidDataException("Import element must have assembly attribute");
            var asm = LoadAssembly(assembly);
            //reader.LoadElementsFromAssembly(asm);
        }

        private Assembly LoadAssembly(string name){
            var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == name);
            if(asm != null) return asm;
            return Assembly.Load(name) ?? throw new InvalidOperationException($"Could not load file or assembly '{name}'");
        }
    }
}

