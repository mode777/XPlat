using System.Xml.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Engine.Serialization;
using XPlat.Graphics;

namespace XPlat.Engine
{
    [SceneElement("atlas")]
    public class SpriteAtlasResource : FileResource, ISerializableResource
    {
        public int Width { get; set; } = 1024;
        public int Height { get; set; } = 1024;
        public SpriteAtlas Atlas => Value as SpriteAtlas;

        public SpriteAtlasResource() : base()
        {
        }

        public void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("src", out var src)) { Filename = reader.ResolvePath(src); Load(); }
            if(el.TryGetAttribute("width", out var w) && int.TryParse(w, out var width)) Width = width;
            if(el.TryGetAttribute("height", out var h) && int.TryParse(h, out var height)) Height = height;
            
            //if(el.TryGetAttribute("watch", out var value) && bool.TryParse(value, out var watch) && watch) { Watch(); }
        }

        protected override object LoadFile()
        {
            var texture = new Texture(Width, Height, TextureUsage.Graphics2d);
            var atlas = new SpriteAtlas(texture);
            var packer = new RectanglePacker((int)texture.Width, (int)texture.Height);
            foreach (var file in CollectImages(Filename))
            {
                using(var img = Image.Load<Rgba32>(file))
                {
                    if (packer.AddRect(img.Width, img.Height, out var x, out var y))
                    {
                        texture.Update(img, x, y);
                        atlas.Add(Path.GetFileNameWithoutExtension(file), x, y, img.Width, img.Height);
                    }
                    else
                    {
                        throw new InvalidDataException("No more room in sprite atlas");
                    }
                }
            }
            return atlas;
        }

        private static string[] extensions = new string[] { ".png", ".jpeg", ".jpg" };

        private IEnumerable<string> CollectImages(string path){
            return Directory.EnumerateFiles(path).Where(x => extensions.Contains(Path.GetExtension(x)) )
            .Concat(Directory.EnumerateDirectories(path).SelectMany(CollectImages));
        }
    }
}