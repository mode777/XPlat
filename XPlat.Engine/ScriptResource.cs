using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;

namespace XPlat.Engine
{
    [SceneElement("script")]
    public class ScriptResource : FileResource, ISerializableResource
    {
        public LuaScript Script => Value as LuaScript;
        public ScriptResource(LuaHost host) : base()
        {
            Value = host.CreateScript();
            Script.OnError += (s,e) => System.Console.WriteLine(e.Message);
        }

        public void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("src", out var src)) { Filename = reader.ResolvePath(src); Load(); }
            if(el.TryGetAttribute("watch", out var value) && bool.TryParse(value, out var watch) && watch) { Watch(); }
        }

        protected override object LoadFile()
        {
            var str = File.ReadAllText(Filename);
            (Value as LuaScript).Load(str);
            return Value;
        }
    }
}