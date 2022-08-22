using System.Numerics;
using System.Xml.Linq;
using NLua;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Engine.Serialization;
using XPlat.Gltf;
using XPlat.WrenScripting;

namespace XPlat.Engine
{

    [SceneElement("node")]
    public class Node : ISceneElement, IDisposable
    {
        public string? Name { get; set; }
        private List<Node> _children = new();
        private List<Component> _components = new();
        private Queue<object> _messagesA = new();
        private Queue<object> _messagesB = new();
        private Queue<object> _messages = null;
        private Dictionary<string, Component> _componentsById = new();
        private bool disposedValue;
        internal Dictionary<Node, CollisionInfo> _collisions = new();
        public Scene Scene { get; }

        public Transform3d Transform { get; set; } = new Transform3d();

        internal Matrix4x4 _globalMatrix;

        public Node? Parent { get; internal set; }
        public string Tag { get; set; }

        

        //internal void Init()
        //{
        //    _globalMatrix = Parent != null ? Transform.GetMatrix() * Parent._globalMatrix : Matrix4x4.Identity;
        //    foreach (var c in GetComponents<Behaviour>().Where(x => x.IsEnabled)) c.Init();
        //    foreach (var c in _children) c.Init();
        //}

        // internal void Update()
        // {
        //     _globalMatrix = Transform.GetMatrix() * Parent._globalMatrix;
        //     foreach (var c in GetComponents<Behaviour>().Where(x => x.IsEnabled))
        //     {
        //         c.Update();
        //     }
        //     foreach (var c in _children)
        //     {
        //         c.Update();
        //     }
        // }

        public Node(Scene scene) 
        { 
            Scene = scene ?? throw new ArgumentNullException(nameof(scene));
            _messages = _messagesA;
        }

        public void AddChild(Node node)
        {
            if (!IsChildAllowed(node)) throw new Exception("Child not allowed");
            node.Parent?.RemoveChild(node);
            _children.Add(node);
            node.Parent = this;
        }

        public void RemoveChild(Node node)
        {
            _children.Remove(node);
            node.Parent = null;
        }

        public Node? Find(string name)
        {
            if (Name == name)
            {
                return this;
            }
            foreach (var c in _children)
            {
                var res = c.Find(name);
                if (res != null)
                    return res;
            }
            return null;
        }

        public Node? FindByTag(string tag)
        {
            if (Tag == tag)
            {
                return this;
            }
            foreach (var c in _children)
            {
                var res = c.Find(tag);
                if (res != null)
                    return res;
            }
            return null;
        }

        public void AddComponent(Component comp)
        {
            //if (comp.Node != null) comp.Node.RemoveComponent(comp);
            _components.Add(comp);
            if(comp.Name != null) _componentsById.Add(comp.Name, comp);
            comp.Node = this;
            comp.OnAttach();
        }

        public void RemoveComponent(Component comp)
        {
            if(comp.Name != null) _componentsById.Remove(comp.Name);
            this._components.Remove(comp);
        }

        

        public T? GetComponent<T>() where T : Component => _components.FirstOrDefault(x => x is T) as T;
        public Component? GetComponent(Type t) => _components.FirstOrDefault(x => x.GetType() == t);
        public Component? GetComponentByName(string id) => _componentsById.TryGetValue(id, out var v) ? v : null;
        public WrenObjectHandle? GetWrenComponent(string name) {
            var c = GetComponentByName(name) as WrenScriptComponent;
            if(c == null) return null;
            //if(c.InstanceHandle == null) c.Instantiate();
            return c.InstanceHandle;
        } 
        public WrenObjectHandle? GetWrenComponent() {
            var c = GetComponent<WrenScriptComponent>();
            if(c == null) return null;
            //if(c.InstanceHandle == null) c.Instantiate();
            return c.InstanceHandle;
        } 
        public LuaTable? GetLuaComponent(string name) => (GetComponentByName(name) as LuaScriptComponent)?.Instance?.Table;
        public LuaTable? GetLuaComponent() => GetComponent<LuaScriptComponent>()?.Instance?.Table;
        public IEnumerable<LuaTable> GetLuaComponents(string name) => GetComponents<LuaScriptComponent>().Where(x => x.Name == name).Select(x => x.Instance.Table);
        public IEnumerable<T> GetComponents<T>() where T : Component => _components.Where(x => x is T).Cast<T>();
        public IEnumerable<Component> GetComponents(string type) => _components.Where(x => x.GetType().Name == type);
        public Component? GetComponent(string type) => _components.FirstOrDefault(x => x.GetType().Name == type);
        public IEnumerable<Component> Components => _components;
        public IEnumerable<Node> Children => _children;
        private bool IsChildAllowed(Node node) => Parent == null || (Parent.IsChildAllowed(node) && node != this);

        private void ParseGltfNode(GltfNode gltf)
        {
            Name = gltf.Name;
            Transform = gltf.Transform;

            if (gltf.HasLight)
            {
                AddComponent(new LightComponent
                {
                    Light = gltf.ReadLight()
                });
            }
            if (gltf.HasMesh)
            {
                AddComponent(new MeshComponent
                {
                    Mesh = gltf.ReadMesh()
                });
            }
            if (gltf.HasCamera)
            {
                AddComponent(new CameraComponent
                {
                    Camera = gltf.ReadCamera()
                });
            }

            foreach (var c in gltf.Children)
            {
                var node = new Node(Scene);
                AddChild(node);
                node.ParseGltfNode(c);
            }
        }

        public void Parse(XElement el, SceneReader reader)
        {
            if (el.TryGetAttribute("src", out var src))
            {
                var split = src.Split(':');
                if (split.Length > 1)
                {
                    var node = reader.LoadGltfNode(split[0], split[1]);
                    ParseGltfNode(node);
                }
                else
                {
                    var scene = reader.LoadGltfScene(split[0]);
                    Name = scene.Name;
                    foreach (var n in scene.Nodes)
                    {
                        var node = new Node(Scene);
                        AddChild(node);
                        node.ParseGltfNode(n);
                    }
                }
            }

            if(el.TryGetAttribute("template", out var template)){
                var t = reader.Scene.Templates[template];
                this.CloneFrom(t);
            }

            if (el.TryGetAttribute("name", out var name)) Name = name;
            if (el.TryGetAttribute("tag", out var tag)) Tag = tag;
            if (el.TryGetAttribute("translate", out var translate)) Transform.TranslationVector = translate.Vector3();
            if (el.TryGetAttribute("rotate", out var rotate)) Transform.SetRotationDeg(rotate.Vector3());

            if (el.TryGetAttribute("scale", out var scale)) Transform.ScaleVector = scale.Vector3();

            foreach (var c in el.Element("components")?.Elements() ?? Enumerable.Empty<XElement>())
            {
                ParseComponent(c, reader);
            }

            foreach (var c in el.Elements())
            {
                if(c.Name == "node"){
                    if (c.TryGetAttribute("name", out var n))
                    {
                        var child = Children.FirstOrDefault(x => x.Name == n);
                        if (child != null)
                        {
                            child.Parse(c, reader);
                            continue;
                        }
                    }
                    var node = new Node(Scene);
                    node.Parse(c, reader);
                    AddChild(node);
                } else if(c.Name == "components") {
                    foreach (var co in c.Elements())
                    {
                        ParseComponent(co, reader);
                    }
                } else {
                    ParseComponent(c, reader);
                }
            }
        }

        private void ParseComponent(XElement c, SceneReader reader){
            var cname = c.TryGetAttribute("name", out var cid) ? cid : null;

            Component component = null;
            if(cname != null) component = GetComponentByName(cname);
            else {
                var target = reader.GetTargetType(c);
                component = GetComponent(target);
            }
            
            if (component != null)
            {
                component.Parse(c, reader);
            }
            else
            {
                component = reader.ReadElement(c) as Component ?? throw new InvalidDataException($"{reader.GetTargetType(c).Name} is not a component");
                component.Name = cname;
                AddComponent(component);
            }
        }

        public void ResetCollisions() => _collisions.Clear();
        public void AddCollision(CollisionInfo info){
            _collisions[info.Other] = info;
        }
        public IEnumerable<CollisionInfo> Collisions => _collisions.Values;
        public IEnumerable<object> Messages => _messages;
        public bool TryGetCollision(Node n, out CollisionInfo info) => _collisions.TryGetValue(n, out info);


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

        public Node Clone(Transform3d transform = null){
            var node = new Node(Scene);
            node.CloneFrom(this, transform);
            return node;
        }

        private void CloneFrom(Node source, Transform3d transform = null){
            transform = transform != null ? new Transform3d(transform) : new Transform3d();

            Transform = transform;

            foreach (var comp in source.Components)
            {
                var cc = comp.Clone(this);                
                AddComponent(cc);
            }

            foreach (var child in source.Children)
            {
                var cc = child.Clone();
                AddChild(cc);
            }
        }

        public void SendMessage(object message){
            _messages.Enqueue(message);
        }

        public Queue<object> GetMessages(){
            if(_messages == _messagesA){
                _messages = _messagesB;
                return _messagesA;
            } else {
                _messages = _messagesA;
                return _messagesB;
            }
        }

        //public 

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

