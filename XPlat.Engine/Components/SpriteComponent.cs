using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.Graphics;

namespace XPlat.Engine.Components
{
    [SceneElement("sprite")]
    public class SpriteComponent : Behaviour
    {
        public SpriteSource Sprite { get; set; }
        public SpriteAtlasResource Resource { 
            get => _resource; 
            private set { 
                _resource = value;
                _resource.Changed += (s,a) => reload = true;
            } 
        }

        private bool reload;
        private SpriteAtlasResource _resource;

        public override void Parse(XElement el, SceneReader reader)
        {
            if (el.TryGetAttribute("res", out var res)) { 
                var split = res.Split(':');
                var resName = split[0];
                var resId = split[1];
                Resource = (SpriteAtlasResource)reader.Scene.Resources.Load(resName);
                Sprite = Resource.Atlas[resId] ?? throw new InvalidDataException($"Sprite '{resId}' not found");
            }
            else if (el.TryGetAttribute("src", out var src)) throw new NotImplementedException("Src attribute is not yet supported for sprites");
            else throw new InvalidDataException("script resource needs 'ref' attribute");

            base.Parse(el, reader);
        }

        public override void Update()
        {
        }
    }
}

