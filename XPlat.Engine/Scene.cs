using System.Xml.Linq;
using XPlat.Core;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;

namespace XPlat.Engine
{
    [SceneElement("scene")]
    public class Scene : ISceneElement, IDisposable
    {
        private bool disposedValue;

        public Scene()
        {
            RootNode = new Node(this);
            Resources = new ResourceManager();
            SetupLua();
        }

        private void SetupLua(){
            LuaHost = new LuaHost();
            LuaHost.ImportNamespace(nameof(XPlat)+ "." + nameof(Core));
        }

        public Node FindNode(string name) => RootNode.Find(name);

        public Scene(Node rootNode) 
        {
            this.RootNode = rootNode;
        }

        public Node RootNode { get; private set; }
        public ResourceManager Resources { get; }
        public LuaHost LuaHost { get; private set; }

        public void Init() => RootNode.Init();
        public void Update() 
        { 
            RootNode.Update(); 
            foreach (var res in Resources)
            {
                if(res is FileResource f && f.FileChanged){
                    f.Load();
                }
            }
        }

        public void Parse(XElement el, SceneReader reader)
        {
            var resources = el.Element("resources");
            if(resources != null){
                foreach (var r in resources.Elements())
                {
                    var type = reader.GetTargetType(r);
                    if(type == typeof(ScriptResource)){
                        if(r.TryGetAttribute("name", out var id)){
                            var script = new ScriptResource(id, null, LuaHost);
                            script.Parse(r, reader);
                            Resources.Store(script);
                        } else {
                            throw new InvalidDataException("Script needs a name attribute");
                        }
                    }
                    if(type == typeof(SpriteAtlasResource)){
                        if(r.TryGetAttribute("name", out var id)){
                            var atlas = new SpriteAtlasResource(id, null);
                            atlas.Parse(r, reader);
                            Resources.Store(atlas);
                        } else {
                            throw new InvalidDataException("Script needs a name attribute");
                        }
                    }
                }
            }

            foreach(var i in el.Elements("import")){
                reader.ReadElement(i);
            }
            var rootEl = el.Element("node") ?? throw new InvalidDataException("A scene needs a root note element");
            RootNode.Parse(rootEl, reader);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    RootNode.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}