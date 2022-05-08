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

        public int Size { get; }

        public SpriteBuffer(Texture texture, int capacity)
        {
            this.Size = capacity;
            this.Texture = texture;
            quads = new Quad[capacity];
            this.defaultRect = new Rectangle(0,0,texture.Width, texture.Height);
        }

        // public int Add(SpriteSource source = null, float x = 0, float y = 0, float rot = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, Color? color = null){
        //     var id = Count;
        //     Set(id, source, x, y, rot, sx, sy, ox, oy, color);
        //     Count++;
        //     return id;
        // }

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

        // public void Rotate(int id, float r){
        //     var s = MathF.Sin(r);
        //     var c = MathF.Cos(r);
        //     int ox = GetX(id), oy = GetY(id);
        //     int x,y;

        //     x = quads[id].A.X - ox;
        //     y = quads[id].A.Y - oy;            
        //     quads[id].A.X = (short)(c*x-s*y+ox);
        //     quads[id].A.Y = (short)(s*x-c*y+oy);

        //     x = quads[id].B.X - ox;
        //     y = quads[id].B.Y - oy;            
        //     quads[id].B.X = (short)(c*x-s*y+ox);
        //     quads[id].B.Y = (short)(s*x-c*y+oy);

        //     x = quads[id].C.X - ox;
        //     y = quads[id].C.Y - oy;            
        //     quads[id].C.X = (short)(c*x-s*y+ox);
        //     quads[id].C.Y = (short)(s*x-c*y+oy);

        //     x = quads[id].D.X - ox;
        //     y = quads[id].D.Y - oy;            
        //     quads[id].D.X = (short)(c*x-s*y+ox);
        //     quads[id].D.Y = (short)(s*x-c*y+oy);
        // }

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

        public void SetColor(int id, byte r, byte g, byte b, byte a){
            var c= new Color(r,g,b,a);
            quads[id].A.Color = c;
            quads[id].B.Color = c;
            quads[id].C.Color = c;
            quads[id].D.Color = c;
        }

        public void Unset(int id){
            quads[id] = new Quad();
        }

        public void Set(int id, SpriteSource source = null, float x = 0, float y = 0, float rot = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, Color? color = null){
            if(id >= Size) throw new IndexOutOfRangeException("Invalid sprite index or buffer full");
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

