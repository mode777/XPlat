using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;
using XPlat.Core;
using XPlat.Graphics;

namespace XPlat.SampleHost
{
    public class SpriteAtlasApp : ISdlApp
    {
        public SpriteAtlasApp(IPlatform platform)
        {
            this.platform = platform;
            platform.AutoClear = true;
        }

        public async void Init()
        {
            var texture = new Texture(1024,1024);
            atlas = new SpriteAtlas(texture);
            var packer = new RectanglePacker((int)texture.Size.X, (int)texture.Size.Y);
            foreach (var file in CollectImages("assets/sprites/space"))
            {
                using(var img = Image.Load<Rgba32>(file))
                {
                    if (packer.AddRect(img.Width, img.Height, out var x, out var y))
                    {
                        //t.Position = new Vector2(x, y);
                        texture.Update(img, x, y);
                        atlas.Add(Path.GetFileName(file), x, y, img.Width, img.Height);
                    }
                    else
                    {
                        throw new InvalidDataException("No more room in sprite atlas");
                    }
                }
            }
            
            spriteBatch = new SpriteBatch();
            transform = new Transform3d();
        }

        private static string[] extensions = new string[] { ".png", ".jpeg", ".jpg" };
        private SpriteBatch spriteBatch;
        private Transform3d transform;
        private SpriteAtlas atlas;
        private readonly IPlatform platform;

        private IEnumerable<string> CollectImages(string path){
            return Directory.EnumerateFiles(path).Where(x => extensions.Contains(Path.GetExtension(x)) )
            .Concat(Directory.EnumerateDirectories(path).SelectMany(CollectImages));
        }

        float r = 0;

        public void Update()
        {
            if(Input.IsKeyDown(Key.A)) transform.RotateDeg(0,0,1);

            spriteBatch.Begin((int)platform.RendererSize.X, (int)platform.RendererSize.Y);
            spriteBatch.SetSprite(atlas["playerShip2_red.png"]);
            spriteBatch.Draw(150, 150);
            spriteBatch.SetSprite(atlas["playerShip1_green.png"]);
            Matrix4x4 m = transform.GetMatrix();
            spriteBatch.Draw(ref m);
            spriteBatch.End();
        }
    }
}