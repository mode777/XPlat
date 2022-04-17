using System.Xml.Linq;
using GLES2;
using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;

namespace XPlat.Engine
{
    internal enum NodeTaskType {
        Insert,
        Delete
    }

    internal class NodeLifecycleTask {

        public NodeLifecycleTask(Node parent, Node target, NodeTaskType type)
        {
            Parent = parent;
            Target = target;
            Type = type;
        }
        public Node Parent;
        public Node Target;
        public NodeTaskType Type;
    }

    [SceneElement("scene")]
    public class Scene : ISceneElement, IDisposable
    {
        private bool disposedValue;
        private List<ISubSystem> _subSystems = new();
        private List<IRenderPass> _renderPasses = new();
        private Queue<NodeLifecycleTask> _nodeTasks = new();
        public Node RootNode { get; private set; }
        public ResourceManager Resources { get; } = new();
        public LuaHost LuaHost { get; private set; }
        public TemplateCollection Templates { get; set; } = new();

        public Scene(SceneConfiguration config)
        {
            RootNode = new Node(this);
            SetupLua();
            config?.Apply(this);
        }

        private void SetupLua(){
            LuaHost = new LuaHost();
            LuaHost.ImportNamespace("System.Numerics");
            LuaHost.ImportNamespace(nameof(XPlat)+ "." + nameof(Core));
        }

        public Node FindNode(string name) => RootNode.Find(name);

        public void RegisterSubsystem(ISubSystem sub)
        {
            _subSystems.Add(sub);
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
            foreach (var sub in _subSystems)
            {
                sub.Init();
            }
            Visit(RootNode, UpdateTransforms);
        }
        public void Update() 
        { 
            ExecuteLifecycleTasks();
            Visit(RootNode, UpdateTransforms);
            foreach (var sub in _subSystems)
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

        public Node Instantiate(Node template, Node parent = null, Transform3d transform = null){
            var cl = template.Clone(transform);
            ScheduleForInsert(cl, parent ?? RootNode);
            return cl;
        }

        private void ExecuteLifecycleTasks(){
            while(_nodeTasks.TryDequeue(out var t)){
                switch (t.Type)
                {
                    case NodeTaskType.Insert:
                        t.Parent.AddChild(t.Target);
                        break;
                    case NodeTaskType.Delete:
                        t.Parent.RemoveChild(t.Target);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ScheduleForInsert(Node n, Node parent){
            _nodeTasks.Enqueue(new NodeLifecycleTask(parent, n, NodeTaskType.Insert));
        }

        private void ScheduleForDelete(Node n){
            var p = n.Parent ?? throw new InvalidOperationException("Can only delete node which is part of tree");
            _nodeTasks.Enqueue(new NodeLifecycleTask(p, n, NodeTaskType.Delete));
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
            ParseImports(el, reader);
            ParseConfiguration(el, reader);
            ParseResources(el, reader);
            ParseTemplates(el, reader);

            var rootEl = el.Element("node") ?? throw new InvalidDataException("A scene needs a root note element");
            RootNode.Parse(rootEl, reader);
        }

        private void ParseTemplates(XElement el, SceneReader reader)
        {
            var templates = el.Element("templates");
            if (templates != null) Templates.Parse(templates, reader);
        }

        private static void ParseImports(XElement el, SceneReader reader)
        {
            foreach (var i in el.Elements("import"))
            {
                reader.ReadElement(i);
            }
        }

        private void ParseResources(XElement el, SceneReader reader)
        {
            var resources = el.Element("resources");
            if (resources != null)
            {
                foreach (var r in resources.Elements())
                {
                    var type = reader.GetTargetType(r);
                    if (type == typeof(ScriptResource))
                    {
                        if (r.TryGetAttribute("name", out var id))
                        {
                            var script = new ScriptResource(id, null, LuaHost);
                            script.Parse(r, reader);
                            Resources.Store(script);
                        }
                        else
                        {
                            throw new InvalidDataException("Script needs a name attribute");
                        }
                    }
                    if (type == typeof(SpriteAtlasResource))
                    {
                        if (r.TryGetAttribute("name", out var id))
                        {
                            var atlas = new SpriteAtlasResource(id, null);
                            atlas.Parse(r, reader);
                            Resources.Store(atlas);
                        }
                        else
                        {
                            throw new InvalidDataException("Script needs a name attribute");
                        }
                    }
                }
            }
        }

        private void ParseConfiguration(XElement el, SceneReader reader){
            if(el.TryGetAttribute("configuration", out var template)) {
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
                        throw new InvalidDataException($"No app configuration found called '{template}'");
                }
            } else {
                // TODO: Load custom pipeline config
                new SceneConfiguration3d(reader.Services.GetRequiredService<IPlatform>())
                    .Apply(this);
            }
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