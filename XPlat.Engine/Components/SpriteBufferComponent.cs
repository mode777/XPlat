using System.Numerics;
using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.Graphics;

namespace XPlat.Engine.Components
{
    [SceneElement("spritebuffer")]
    public class SpriteBufferComponent : Component
    {
        public SpriteAtlasResource Resource { 
            get => _resource; 
            private set { 
                _resource = value;
                _resource.Changed += (s,a) => reload = true;
            } 
        }

        private bool reload;
        private SpriteAtlasResource _resource;
        public SpriteAtlas Atlas => _resource.Atlas;
        public Vector2 Origin = Vector2.Zero;
        public float OriginX { get => Origin.X; set => Origin.X = value; }
        public float OriginY { get => Origin.Y; set => Origin.Y = value; }
        public SpriteBuffer Buffer { get; set; }

        public override void Parse(XElement el, SceneReader reader)
        {
            if (el.TryGetAttribute("res", out var res)) { 
                Resource = (SpriteAtlasResource)reader.Scene.Resources.Load(res);
            }
            else if (el.TryGetAttribute("src", out var src)) throw new NotImplementedException("Src attribute is not yet supported for spritesbuffers");
            else throw new InvalidDataException("script resource needs 'ref' attribute");
            if(el.TryGetAttribute("origin", out var o)) Origin = o.Vector2();
            Buffer = new SpriteBuffer(Resource.Atlas.Texture, 64);

            base.Parse(el, reader);
        }
    }
}

