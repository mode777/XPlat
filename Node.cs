using SharpGLTF.Transforms;

namespace net6test
{
    public class Node 
    {
        private List<Node> _children = new();
        private List<Component> _components = new();
        public Node? Parent { get; private set; }
        public AffineTransform Transform { get; set; } = new AffineTransform();

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

    public class Component
    {
        public virtual void Init(){}
        public virtual void Update(){}

        public Node? Node { get; set; }
    }

    public class RendererComponent : Component
    {
        public Mesh? Mesh { get; set; }
        public Shader? Shader { get; set; } = Shader.Current;

        public void Draw(){
            if(Mesh != null && Shader != null)
            {
                Shader.Use(Shader);
                Mesh.DrawUsingShader(Shader);
            }
        }

    }
}

