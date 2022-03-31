using System.Numerics;
using System.Xml.Linq;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Engine.Serialization;
using XPlat.Gltf;

namespace XPlat.Engine
{

    [SceneElement("node")]
    public class Node : ISceneElement, IDisposable 
    {
        public string? Name { get; set; }
        private List<Node> _children = new();
        private List<Component> _components = new();
        private bool disposedValue;

        public Transform3d Transform { get; set; } = new Transform3d();

        public Transform3d GetGlobalTransform(){
            var t = Transform;
            var gm = Matrix4x4.Identity;
            while(t != null){
                var m = t.GetMatrix();
                gm *= m;
                t = Parent?.Transform;
            }
            return new Transform3d(gm);
        }

        public Node? Parent { get; private set; }
        public string Tag { get; set; }

        internal void Init()
        {
            foreach(var c in GetComponents<Behaviour>().Where(x => x.IsEnabled)) c.Init();
            foreach(var c in _children) c.Init();
        }

        internal void Update()
        {
            foreach (var c in GetComponents<Behaviour>().Where(x => x.IsEnabled))
            {
                c.Update();
            }
            foreach (var c in _children)
            {
                c.Update();
            }
        }

        public Node(){}
        
        public void AddChild(Node node){
            if(!IsChildAllowed(node)) throw new Exception("Child not allowed");
            node.Parent?.RemoveChild(node);
            _children.Add(node);
            node.Parent = this;
        }

        public void RemoveChild(Node node){
            _children.Remove(node);
            node.Parent = null;
        }

        public Node? Find(string name)
        {
            if(Name == name){
                return this;
            }
            foreach (var c in _children)
            {
                var res = c.Find(name);
                if(res != null) 
                    return res;
            }
            return null;
        }

        public Node? FindByTag(string tag)
        {
            if(Tag == tag){
                return this;
            }
            foreach (var c in _children)
            {
                var res = c.Find(tag);
                if(res != null) 
                    return res;
            }
            return null;
        }

        public void AddComponent(Component comp)
        {
            if(comp.Node != null) comp.Node.RemoveComponent(comp);
            _components.Add(comp);
            comp.Node = this; 
        }

        public void RemoveComponent(Component comp)
        {
            this._components.Remove(comp);
        }

        public T? GetComponent<T>() where T : Component => _components.FirstOrDefault(x => x is T) as T;
        public Component? GetComponent(Type t) => _components.FirstOrDefault(x => x.GetType() == t);
        public IEnumerable<T> GetComponents<T>() where T : Component => _components.Where(x => x is T).Cast<T>();
        public IEnumerable<Component> Components => _components;
        public IEnumerable<Node> Children => _children;
        private bool IsChildAllowed(Node node) => Parent == null || (Parent.IsChildAllowed(node) && node != this);

        private void ParseGltfNode(GltfNode gltf){
            Name = gltf.Name;
            Transform = gltf.Transform;

            if(gltf.HasLight){
                AddComponent(new LightComponent {
                    Light = gltf.ReadLight()
                });
            }
            if(gltf.HasMesh){
                AddComponent(new MeshComponent {
                    Mesh = gltf.ReadMesh()
                });
            }
            if(gltf.HasCamera){
                AddComponent(new CameraComponent{
                    Camera = gltf.ReadCamera()
                });
            }

            foreach(var c in gltf.Children){
                var node = new Node();
                AddChild(node);
                node.ParseGltfNode(c);
            }
        }

        public void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("src", out var src)){
                var split = src.Split(':');
                if(split.Length > 1){
                    var node = reader.LoadGltfNode(split[0], split[1]);
                    ParseGltfNode(node);
                } else {
                    var scene = reader.LoadGltfScene(split[0]);
                    Name = scene.Name;
                    foreach (var n in scene.Nodes)
                    {
                        var node = new Node();
                        AddChild(node);
                        node.ParseGltfNode(n);
                    }
                }

            }
            if(el.TryGetAttribute("name", out var name)) Name = name;
            if(el.TryGetAttribute("tag", out var tag)) Tag = tag;
            if(el.TryGetAttribute("translate", out var translate)) Transform.Translation = translate.Vector3();
            if(el.TryGetAttribute("rotate", out var rotate)) Transform.RotateDeg(rotate.Vector3());
            
            if(el.TryGetAttribute("scale", out var scale)) Transform.Scale = scale.Vector3();

            foreach(var c in el.Element("components")?.Elements() ?? Enumerable.Empty<XElement>()){
                var target = reader.GetTargetType(c);
                var component = GetComponent(target);
                if(component != null) {
                    component.Parse(c, reader);
                }
                else {
                    component = reader.ReadElement(c) as Component ?? throw new InvalidDataException($"{target.Name} is not a component");
                    AddComponent(component);
                }
            }

            foreach(var c in el.Elements("node")){
                if(c.TryGetAttribute("name", out var n)){
                    var child = Children.FirstOrDefault(x => x.Name == n);
                    if(child != null) {
                        child.Parse(c, reader);
                        continue;
                    }
                } 
                var node = (Node)reader.ReadElement(c);
                AddChild(node);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var c in this.Components)
                    {
                        c.Dispose();
                    }
                    foreach (var c in Children)
                    {
                        c.Dispose();
                    }
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

