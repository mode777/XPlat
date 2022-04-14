using System.Reflection;
using System.Xml.Linq;
using XPlat.Gltf;
using XPlat.Graphics;

namespace XPlat.Engine.Serialization
{
    public class SceneReader
    {

        public Dictionary<string, Type> SceneElements { get; } = new Dictionary<string, Type>();
        public XElement XElement { get; private set; }

        public string Directory => root;

        public Scene Scene { get; private set; }
        public IServiceProvider Services { get; }

        public SceneReader(IServiceProvider provider)
        {
            this.Services = provider;
            LoadElementsFromAssembly(typeof(SceneReader).Assembly);
        }

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

        public string ResolvePath(string path)
        {
            return Path.Combine(root, path);
        }

        private string root;



        private Scene Read(XDocument doc)
        {
            var sceneEl = doc.Element("scene") ?? throw new InvalidDataException("Root element 'scene' not found");
            
            Scene = new Scene(null);
            Scene.Parse(sceneEl, this);
            return Scene;
        }

        public Scene Read(string path)
        {
            this.root = Path.GetFullPath(Path.GetDirectoryName(path));
            var doc = XDocument.Load(path);
            return Read(doc);
        }

        public Type GetTargetType(XElement el) => SceneElements.TryGetValue(el.Name.LocalName, out var type)
            ? type
            : throw new InvalidDataException($"Unknown element {el.Name.LocalName}");

        public ISceneElement ReadElement(XElement el)
        {
            var type = GetTargetType(el);
            var inst = (ISceneElement)Activator.CreateInstance(type);
            //if(inst is Scene scn) this.Scene = scn;
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

