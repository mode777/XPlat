using System.Xml.Linq;
using NLua;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;

namespace XPlat.Engine.Components
{
    [SceneElement("lua")]
    public class LuaScriptComponent : Behaviour
    {
        public ScriptResource Resource { 
            get => _resource; 
            private set { 
                _resource = value;
                _resource.Changed += (s,a) => reload = true;
            } 
        }

        public string Name { get; private set; }
        public LuaScriptInstance Instance { get; private set; }
        public LuaTable Arguments { get; private set; }

        private bool reload;
        private bool initialized;
        private ScriptResource _resource;

        public override void Init()
        {
            Instantiate();
        }

        private void Instantiate(){
            if(Resource == null) return;
            Instance = Resource.Script.Instantiate(Node, Arguments);
            if(Instance == null) return;
            Instance.OnError += (s,e) => System.Console.WriteLine($"({Name}):{e.Message}");
            reload = false;
            initialized = false;
        }

        private void Initialize(){
            Instance?.Init();
            initialized = true;
        }

        public override void Parse(XElement el, SceneReader reader)
        {
            if (el.TryGetAttribute("res", out var res)) { 
                Resource = (ScriptResource)reader.Scene.Resources.Load(res);
                Name = res;
            }
            else throw new InvalidDataException("script resource needs 'ref' attribute");

            if(el.TryGetAttribute("args", out var args)) Arguments = reader.Scene.LuaHost.ParseTable(args) ?? throw new InvalidDataException("Unable to parse script arguments");
            if (el.TryGetAttribute("src", out var src)) throw new NotImplementedException("Src attribute is no longer supported for scripts");
            base.Parse(el, reader);
        }

        public override void Update()
        {
            if (reload) Instantiate();
            if(!initialized) Initialize();
            Instance?.Update();
        }

        public override void OnCollision(CollisionInfo info)
        {
            Instance?.Call("onCollision", info);
        }
    }
}

