using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.WrenScripting;

namespace XPlat.Engine
{
    [SceneElement("wren-script")]
    public class WrenScriptResource : FileResource, ISerializableResource
    {
        private readonly WrenVm vm;
        public WrenScriptResource(WrenVm vm)
        {
            this.vm = vm;
        }

        public void Parse(XElement el, SceneReader reader)
        {
            if (el.TryGetAttribute("name", out var name)) { Name = name; }
            if (el.TryGetAttribute("src", out var src)) { Filename = reader.ResolvePath(src); Load(); }
            if (el.TryGetAttribute("watch", out var value) && bool.TryParse(value, out var watch) && watch) { Watch(); }
        }

        protected override object LoadFile()
        {
            var str = File.ReadAllText(Filename);
            vm.Interpret(Name, str);
            return null;
        }
        public string Name { get; set; }
    }
}