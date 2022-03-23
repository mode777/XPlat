namespace net6test
{
    public class Component
    {
        public Transform? Transform => Node?.Transform;
        public virtual void Init(){}
        public virtual void Update(){}

        public Node? Node { get; set; }
    }
}

