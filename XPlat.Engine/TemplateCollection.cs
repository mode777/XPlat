using System.Collections;
using System.Xml.Linq;
using XPlat.Engine.Serialization;

namespace XPlat.Engine
{
    public class TemplateCollection : ISceneElement, IEnumerable<Node>
    {
        private Dictionary<string, Node> _templates = new();


        public void Parse(XElement el, SceneReader reader)
        {
            foreach(var n in el.Elements("node")){
                var node = new Node(reader.Scene);
                node.Parse(n, reader);
                Add(node);
            }
        }

        public void Add(Node n){
            _templates[n.Name] = n;
        }

        public Node this[string name] { get => this._templates[name] ?? throw new KeyNotFoundException(name);  }
        IEnumerator IEnumerable.GetEnumerator() => _templates.Values.GetEnumerator();
        public IEnumerator<Node> GetEnumerator() => _templates.Values.GetEnumerator();
    }
}