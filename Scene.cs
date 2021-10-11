namespace net6test
{
    public class Scene
    {
        public Scene()
        {
            RootNode = new Node();
        }

        public Node FindNode(string name) => RootNode.Find(name);

        public Node RootNode { get; }
    }
}