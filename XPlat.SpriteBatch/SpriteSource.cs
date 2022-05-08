using System;
using System.Drawing;

namespace XPlat.Graphics
{
    public class SpriteSource
    {
        public readonly Rectangle Rectangle;
		public readonly Texture Texture;
        public int Width => Rectangle.Width;
        public int Height => Rectangle.Height;

        public SpriteSource(Texture texture, Rectangle rect)
        {
            Texture = texture ?? throw new NullReferenceException(nameof(texture));
            Rectangle = rect;
        }

		public SpriteSource(Texture texture)
        {
            Texture = texture ?? throw new NullReferenceException(nameof(texture));
            Rectangle = new Rectangle(0,0,texture.Width,texture.Height);
        }
    }

}