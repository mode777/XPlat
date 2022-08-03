using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.WrenScripting;

namespace XPlat.Engine.Components
{
    [SceneElement("wren")]
    public class WrenScriptComponent : Behaviour
    {
        private readonly WrenVm vm;
        private readonly ResourceManager resources;
        private WrenObjectHandle instance;

        public WrenScriptComponent(WrenVm vm, ResourceManager resources)
        {
            this.resources = resources;
            this.vm = vm;
        }

        public override void Init()
        {
            Instantiate();
            instance.Call("init()");
        }

        public void Instantiate()
        {
            var split = Resource.Split(':');
            var clas = vm.GetObject(split[0], split[1]);
            this.instance = clas.CallForObject("new(_)", Node);
        }

        public override void Update()
        {
            instance.Call("update()");
        }

        public override void Parse(XElement el, SceneReader reader)
        {
            if (el.TryGetAttribute("res", out var res)) Resource = res;
            else throw new InvalidDataException("Wren script needs 'res' attribute");
            //if (el.TryGetAttribute("args", out var args)) Arguments = host.ParseTable(args) ?? throw new InvalidDataException("Unable to parse script arguments");
            //if (el.TryGetAttribute("src", out var src)) throw new NotImplementedException("Src attribute is no longer supported for scripts");
            base.Parse(el, reader);
        }

        public string Resource { get; set; }
    }
}

