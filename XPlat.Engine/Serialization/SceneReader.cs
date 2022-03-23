using System.Reflection;
using System.Xml.Linq;
using XPlat.Gltf;
using XPlat.Graphics;

namespace XPlat.Engine.Serialization
{
    public class SceneReader
    {

        public static Scene Load(string path)
        {
            var doc = XDocument.Load(path);
            var reader = new SceneReader(Path.GetFullPath(Path.GetDirectoryName(path)));
            return reader.Read(doc);
        }

        public Dictionary<string, Type> SceneElements { get; } = new Dictionary<string, Type>();
        public XElement XElement { get; private set; }

        public GltfNode LoadGltfNode(string file, string path)
        {
            var scene = GltfReader.Load(ResolvePath(file));
            return scene.FindNode(path);
        }

        public GltfScene LoadGltfScene(string file)
        {
            var scene = GltfReader.Load(ResolvePath(file));
            return scene;
        }

        private string ResolvePath(string path){
            return Path.Combine(root, path);
        }

        private readonly string root;

        private SceneReader(string root)
        {
            this.root = root;
            LoadElementsFromAssembly(typeof(SceneReader).Assembly);
        }

        private Scene Read(XDocument doc)
        {
            var scene = doc.Element("scene") ?? throw new InvalidDataException("Root element 'scene' not found");
            return (Scene)ReadElement(scene);
        }

        public Type GetTargetType(XElement el) => SceneElements.TryGetValue(el.Name.LocalName, out var type) 
            ? type 
            : throw new InvalidDataException($"Unknown element {el.Name.LocalName}");

        public ISceneElement ReadElement(XElement el)
        {
            var type = GetTargetType(el);
            var inst = (ISceneElement)Activator.CreateInstance(type);
            inst.Parse(el, this);
            return inst;
        }

        public void LoadElementsFromAssembly(Assembly asm)
        {
            var pairs = asm.ExportedTypes
                .Where(x => typeof(ISceneElement).IsAssignableFrom(x))
                .Select(x => new KeyValuePair<string, Type>(x.GetCustomAttribute<SceneElementAttribute>()?.Name ?? x.Name, x));

            foreach (var kv in pairs)
            {
                SceneElements.Add(kv.Key, kv.Value);
            }
        }


    }
}

