using System.Xml.Linq;
using GLES2;
using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;

namespace XPlat.Engine
{
    [SceneElement("scene")]
    public class Scene : ISceneElement, IDisposable
    {
        private bool disposedValue;
        private List<IInitSubSystem> _initSystems = new List<IInitSubSystem>();
        private List<IUpdateSubSystem> _updateSystems = new List<IUpdateSubSystem>();
        private List<IRenderPass> _renderPasses = new List<IRenderPass>();
        public Node RootNode { get; private set; }
        public ResourceManager Resources { get; }
        public LuaHost LuaHost { get; private set; }

        public Scene(SceneConfiguration config)
        {
            RootNode = new Node(this);
            Resources = new ResourceManager();
            SetupLua();
            config?.Apply(this);
        }

        private void SetupLua(){
            LuaHost = new LuaHost();
            LuaHost.ImportNamespace("System.Numerics");
            LuaHost.ImportNamespace(nameof(XPlat)+ "." + nameof(Core));
        }

        public Node FindNode(string name) => RootNode.Find(name);

        public void RegisterInitSubsystem(IInitSubSystem sub)
        {
            _initSystems.Add(sub);
        }

        public void RegisterUpdateSubsystem(IUpdateSubSystem sub)
        {
            _updateSystems.Add(sub);
        }

        public void RegisterRenderPass(IRenderPass pass)
        {
            _renderPasses.Add(pass);
        }



        //private ProcessNode _OnInit;
        //private ProcessNode _OnUpdate;
        private delegate void ProcessNode(Node n);
        private delegate void ProcessComponent(Node n, Component c);

        public void Init() {
            Visit(RootNode, UpdateTransforms);
            foreach (var sub in _initSystems)
            {
                sub.BeforeInit();
                Visit(RootNode, sub.OnInit);
                sub.AfterInit();
            }
        }
        public void Update() 
        { 
            Visit(RootNode, UpdateTransforms);
            foreach (var sub in _updateSystems)
            {
                sub.BeforeUpdate();
                Visit(RootNode, sub.OnUpdate);
                sub.AfterUpdate();
            }

            foreach (var res in Resources)
            {
                if(res is FileResource f && f.FileChanged){
                    f.Load();
                }
            }
        }

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

            foreach (var pass in _renderPasses)
            {
                pass.StartFrame();
                Visit(RootNode, pass.OnRender);
                pass.FinishFrame();
            }
        }

        public void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("template", out var template)) {
                switch(template){
                    case "2d":
                        new SceneConfiguration2d(reader.Services.GetRequiredService<IPlatform>())
                            .Apply(this);
                        break;
                    case "3d":
                        new SceneConfiguration3d(reader.Services.GetRequiredService<IPlatform>())
                            .Apply(this);
                        break;
                    default:
                        throw new InvalidDataException($"No app template found called '{template}'");
                }
            } else {
                // TODO: Load custom pipeline config
                new SceneConfiguration3d(reader.Services.GetRequiredService<IPlatform>())
                    .Apply(this);
            }

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