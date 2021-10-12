using SharpGLTF.Transforms;

namespace net6test
{
    public class Node 
    {
        public string? Name { get; set; }
        private List<Node> _children = new();
        private List<Component> _components = new();
        public Node? Parent { get; private set; }
        public Transform Transform { get; set; } = new Transform();

        internal void Update()
        {
            foreach (var c in _components)
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

        public Node? Find(string name) => Name == name ? this : _children.Select(x => x.Find(name)).FirstOrDefault();

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

        public void Draw()
        {
            GetComponent<RendererComponent>()?.Draw();
            foreach (var child in _children)
            {
                child.Draw();
            }
        }

        public T? GetComponent<T>() where T : Component => _components.FirstOrDefault(x => x is T) as T;

        private bool IsChildAllowed(Node node) => Parent == null || (Parent.IsChildAllowed(node) && node != this);
    }
}

