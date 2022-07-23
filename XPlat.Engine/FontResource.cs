using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;
using XPlat.NanoVg;

namespace XPlat.Engine
{
    [SceneElement("font")]
    public class FontResource : FileResource, ISerializableResource
    {
        private readonly NVGcontext vg;

        public FontResource(NVGcontext vg) : base()
        {
            this.vg = vg;
        }

        public void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("src", out var src)) { Filename = reader.ResolvePath(src); Load(); }
            if(el.TryGetAttribute("watch", out var value) && bool.TryParse(value, out var watch) && watch) { Watch(); }
        }

        protected override object LoadFile()
        {
            Value = vg.CreateFont(Id, Filename);
            return Value;
        }
    }
}