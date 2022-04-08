using System.Numerics;
using XPlat.Core;
using XPlat.Graphics;

namespace XPlat.SampleHost
{
    public class SpriteSheetEntry {
        public Texture Texture { get; set; }
        public Vector2 Position { get; set; }
        public string Name { get; set; }
    }

    public class SpriteAtlasApp : ISdlApp
    {
        public SpriteAtlasApp(IPlatform platform)
        {
            this.platform = platform;
            platform.AutoClear = true;
        }

        public async void Init()
        {
            textures = CollectImages("assets/sprites/space")
                .Select(x => new SpriteSheetEntry { 
                    Texture = new Texture(x, TextureUsage.GraphicsPixel), 
                    Name = x 
                }).ToArray();
            var atlas = new SpriteAtlas(1024,1024,textures.Length);
            foreach (var t in textures)
            {
                if(atlas.AddRect((int)t.Texture.Size.X,(int)t.Texture.Size.Y, out var x, out var y)){
                    t.Position = new Vector2(x,y);
                } else {
                    throw new InvalidDataException("No more room in sprite atlas");
                }
                
            }
            spriteBatch = new SpriteBatch();

        }

        private static string[] extensions = new string[] { ".png", ".jpeg", ".jpg" };
        private SpriteSheetEntry[] textures;
        private SpriteBatch spriteBatch;
        private readonly IPlatform platform;

        private IEnumerable<string> CollectImages(string path){
            return Directory.EnumerateFiles(path).Where(x => extensions.Contains(Path.GetExtension(x)) )
            .Concat(Directory.EnumerateDirectories(path).SelectMany(CollectImages));
        }

        public void Update()
        {
            spriteBatch.Begin((int)platform.RendererSize.X, (int)platform.RendererSize.Y);
            foreach (var t in textures)
            {
                spriteBatch.SetTexture(t.Texture);
                spriteBatch.Draw((int)t.Position.X,(int)t.Position.Y);
            }
            spriteBatch.End();
        }
    }
}