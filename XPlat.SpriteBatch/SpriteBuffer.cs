using System;
using System.Drawing;
using System.Numerics;

namespace XPlat.Graphics
{
    public class SpriteBuffer
    {
        public readonly Texture Texture;
        internal readonly Quad[] quads;
        private readonly Rectangle defaultRect;

        public int Capacity { get; }
        public int Count { get; private set; } = 0;

        public SpriteBuffer(Texture texture, int capacity)
        {
            this.Capacity = capacity;
            this.Texture = texture;
            quads = new Quad[capacity];
            this.defaultRect = new Rectangle(0,0,texture.Width, texture.Height);
        }

        public int Add(SpriteSource source = null, float x = 0, float y = 0, float rot = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, Color? color = null){
            var id = Count;
            Set(id, source, x, y, rot, sx, sy, ox, oy, color);
            Count++;
            return id;
        }

        public void Move(int id, int x, int y){
            quads[id].A.X += (short)x;
            quads[id].B.X += (short)x;
            quads[id].C.X += (short)x;
            quads[id].D.X += (short)x;
            quads[id].A.Y += (short)y;
            quads[id].B.Y += (short)y;
            quads[id].C.Y += (short)y;
            quads[id].D.Y += (short)y;
        }

        public int GetX(int id){
            return (int)(0.25f * (quads[id].A.X + 
            quads[id].B.X +
            quads[id].C.X +
            quads[id].D.X));
        }

        public int GetY(int id){
            return (int)(0.25f * (quads[id].A.Y + 
            quads[id].B.Y +
            quads[id].C.Y +
            quads[id].D.Y));
        }

        public void Set(int id, SpriteSource source = null, float x = 0, float y = 0, float rot = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, Color? color = null){
            if(id >= Capacity || id > Count) throw new IndexOutOfRangeException("Invalid sprite index or buffer full");
            if(source != null && source.Texture != Texture) throw new InvalidOperationException("Spritebuffer must only use one texture");
            var r = source?.Rectangle ?? defaultRect;
            var c = color ?? Color.White;
            var cos = MathF.Cos(rot);
            var sin = MathF.Sin(rot);
            var matrix = Matrix3x2.Identity;
            matrix.M11 = sx * cos;
            matrix.M12 = sx * sin;
            matrix.M21 = sx * -sin;
            matrix.M22 = sx * cos;
            matrix.M31 = -ox * sx * cos + -oy * sx * -sin + x;
            matrix.M32 = -ox * sx * sin + -oy * sx * cos + y;
            quads[id] = new Quad(
                Vector2.Transform(new Vector2(0,r.Height), matrix), 
                Vector2.Transform(new Vector2(0,0), matrix), 
                Vector2.Transform(new Vector2(r.Width,0), matrix), 
                Vector2.Transform(new Vector2(r.Width,r.Height), matrix),
                r,
                c
            );
        }
    }
}

