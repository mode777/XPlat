using System.Reflection;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.Gltf;
using XPlat.Graphics;

namespace XPlat.Engine.Serialization
{
    public class SceneReader
    {
        public XElement XElement { get; private set; }

        public string Directory => root;

        public Scene Scene { get; private set; }
        public IServiceProvider Services { get; }
        private readonly TypeRegistry registry;
        public ResourceManager Resources {get;}

        public SceneReader(TypeRegistry registry, IServiceProvider provider, ResourceManager resource)
        {
            this.Resources = resource;
            this.registry = registry;
            Services = provider;
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

        public SceneConfiguration BuildSceneConfigurationFromTemplate(string name)
        {
            if (registry.SceneTemplates.TryGetValue(name, out var type))
            {
                return (SceneConfiguration)Services.GetService(type);
            }
            else
            {
                throw new InvalidDataException($"Template '{name}' not found.");
            }
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

            ParseImports(sceneEl);
            ParseConfiguration(sceneEl);
            ParseResources(sceneEl);
            ParseTemplates(sceneEl);

            Scene.Parse(sceneEl, this);
            return Scene;
        }

        public Scene Read(string path)
        {
            this.root = Path.GetFullPath(Path.GetDirectoryName(path));
            var doc = XDocument.Load(path);
            return Read(doc);
        }

        public Type GetTargetType(XElement el) => registry.SceneElements.TryGetValue(el.Name.LocalName, out var type)
            ? type
            : throw new InvalidDataException($"Unknown element {el.Name.LocalName}");

        public ISceneElement ReadElement(XElement el)
        {
            var type = GetTargetType(el);
            var inst = (ISceneElement)Services.GetService(type);
            //if(inst is Scene scn) this.Scene = scn;
            inst.Parse(el, this);
            return inst;
        }

        private void ParseTemplates(XElement el)
        {
            var templates = el.Element("templates");
            if (templates != null) Scene.Templates.Parse(templates, this);
        }

        private void ParseImports(XElement el)
        {
            foreach (var i in el.Elements("import"))
            {
                ReadElement(i);
            }
        }

        private void ParseConfiguration(XElement el)
        {
            var config = el.Element("configuration");
            if (config != null)
            {
                if (config.TryGetAttribute("template", out var template))
                {
                    BuildSceneConfigurationFromTemplate(template)
                        .Apply(Scene);
                }
            }
            else if (el.TryGetAttribute("configuration", out var template))
            {
                BuildSceneConfigurationFromTemplate(template)
                    .Apply(Scene);
            }
            else
            {
                BuildSceneConfigurationFromTemplate("3d")
                    .Apply(Scene);
            }
        }

        private void ParseResources(XElement el)
        {

            var resourceElems = el.Element("resources");
            if (resourceElems != null)
            {
                foreach (var r in resourceElems.Elements())
                {
                    var type = GetTargetType(r);
                    var resource = Services.GetRequiredService(type) as ISerializableResource ?? throw new Exception("Serializable Resources must implement ISerializableResource interface");

                    if (r.TryGetAttribute("name", out var id))
                    {
                        resource.Id = id;
                        resource.Parse(r, this);
                        Resources.Store(resource);
                    }
                    else
                    {
                        throw new InvalidDataException("Script needs a name attribute");
                    }
                }
            }
        }
    }
}

