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
        //public LuaHost LuaHost { get; private set; }
        public TemplateCollection Templates { get; set; } = new();
        public ResourceManager Resources { get; set; } = new();

        public Scene()
        {
            RootNode = new Node(this);
        }



        public Node FindNode(string name) => RootNode.Find(name);

        public void RegisterSubsystem(ISubSystem sub)
        {
            _subSystems.Add(sub);
        }

        public void RegisterRenderPass(IRenderPass pass)
        {
            _renderPasses.Add(pass);
            pass.OnAttach(this);
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

            // TODO
            // foreach (var res in Resources)
            // {
            //     if(res is FileResource f && f.FileChanged){
            //         f.Load();
            //     }
            // }
        }

        public Node Instantiate(Node template, Node parent = null, Transform3d transform = null){
            parent = parent ?? RootNode; 
            var cl = template.Clone(transform);
            cl.Parent = parent;
            ScheduleForInsert(cl, parent ?? RootNode);
            return cl;
        }

        public void Delete(Node node)
        {
            ScheduleForDelete(node);
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
            var rootEl = el.Element("node") ?? throw new InvalidDataException("A scene needs a root note element");
            RootNode.Parse(rootEl, reader);
        }




        // private void LoadConfigurationTemplate(string template, IPlatform platform){
        //     switch(template){
        //         case "2d":
        //             new SceneConfiguration2d(platform)
        //                 .Apply(this);
        //             break;
        //         case "3d":
        //             new SceneConfiguration3d(platform)
        //                 .Apply(this);
        //             break;
        //         default:
        //             throw new InvalidDataException($"No scene configuration found called '{template}'");
        //     }
        // }

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