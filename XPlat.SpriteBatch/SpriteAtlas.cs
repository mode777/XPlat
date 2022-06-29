using System.Collections.Generic;
using System.Drawing;

namespace XPlat.Graphics
{
    public class SpriteAtlas
    {
        public readonly Texture Texture;
		private readonly Dictionary<string, SpriteSource> sprites = new Dictionary<string, SpriteSource>();

        public SpriteAtlas(Texture texture)
        {
            this.Texture = texture;
        }

		public void Add(string name, int x, int y, int w, int h)
        {
			sprites[name] = new SpriteSource(Texture, new Rectangle(x, y, w, h));
        }

		public SpriteSource this[string name]
		{
			get { return sprites[name]; }
		}

	}

}