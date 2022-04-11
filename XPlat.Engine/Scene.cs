using System.Xml.Linq;
using GLES2;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;

namespace XPlat.Engine
{
    public interface ISubSystem {
        void BeginPass();
        void FinishPass();
        void OnInit(Node n, Component comp);
        void OnUpdate(Node n, Component comp);
    }

    public class BehaviourSubsystem : ISubSystem
    {
        public void BeginPass()
        {
            throw new NotImplementedException();
        }

        public void FinishPass()
        {
            throw new NotImplementedException();
        }

        public void OnInit(Node n, Component comp)
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(Node n, Component comp)
        {
            throw new NotImplementedException();
        }
    }

    [SceneElement("scene")]
    public class Scene : ISceneElement, IDisposable
    {
        private bool disposedValue;

        public Scene()
        {
            RootNode = new Node(this);
            Resources = new ResourceManager();
            SetupLua();
            ConfigurePipeline();
        }

        private void SetupLua(){
            LuaHost = new LuaHost();
            LuaHost.ImportNamespace("System.Numerics");
            LuaHost.ImportNamespace(nameof(XPlat)+ "." + nameof(Core));
        }

        public Node FindNode(string name) => RootNode.Find(name);

        public Node RootNode { get; private set; }
        public ResourceManager Resources { get; }
        public LuaHost LuaHost { get; private set; }

        private event ProcessNode _initFns;
        private event ProcessNode _updateFns;

        private void ConfigurePipeline(){
            _initFns += UpdateTransforms;
            _initFns += InitalizeComponents;

            _updateFns += UpdateTransforms;
            _updateFns += UpdateComponents;
        }

        public void Init() {
            Visit(RootNode, _initFns);
        }
        public void Update() 
        { 
            Visit(RootNode, _updateFns); 
            foreach (var res in Resources)
            {
                if(res is FileResource f && f.FileChanged){
                    f.Load();
                }
            }
        }

        private delegate void ProcessNode(Node n);
        private delegate void ProcessComponent(Component n);
        //private Dictionary<Type, ProcessComponent> _initHandlers = new Dictionary<Type, ProcessComponent>();
        //private Dictionary<Type, ProcessComponent> _updateHandlers = new Dictionary<Type, ProcessComponent>();
        // private void ProcessNodeGeneric(Node node){
        //     foreach (var c in node.Components)
        //     {
        //         if(!c.IsEnabled) continue;
        //         foreach (var kv in handlers)
        //         {
        //             if(kv.Key.IsInstanceOfType(kv.Value)) kv.Value?.Invoke(c);
        //         }
        //     }
        // }

        private void InitalizeComponents(Node node){
            foreach (var c in node.GetComponents<Behaviour>())
            {
                if(c.IsEnabled) c.Init();
            }
        }

        private void UpdateComponents(Node node){
            foreach (var c in node.GetComponents<Behaviour>())
            {
                if(c.IsEnabled) c.Update();
            }
        }

        private void UpdateTransforms(Node node){
            node._globalMatrix = node.Transform.GetMatrix() * node.Parent?._globalMatrix ?? System.Numerics.Matrix4x4.Identity;
        }

        private void Visit(Node node, ProcessNode fn)
        {
            fn?.Invoke(node);
            
            foreach (var n in node.Children){
                Visit(n, fn);
            }
        }

        public void Render(){
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
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