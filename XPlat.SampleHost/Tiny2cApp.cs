using System;
using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Engine;
using XPlat.NanoVg;
using static TinyC2.TinyC2Api;

namespace XPlat.SampleHost
{
    public class Tiny2cApp : ISdlApp
    {
        private readonly IPlatform platform;
        private NVGcontext vg;
        private List<c2Shape> shapes = new List<c2Shape>();
        private c2Manifold man;

        public Tiny2cApp(IPlatform platform)
        {
            this.platform = platform;
        }

        public void Init()
        {
            this.vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS | NVGcreateFlags.NVG_STENCIL_STROKES);

            var circle = new c2Circle
            {
                p = new Vector2(100, 100),
                r = 50
            };
            shapes.Add(circle);

            var capsule = new c2Capsule {
                a = new Vector2(150, 150),
                b = new Vector2(300, 300),
                r = 50
            };
            shapes.Add(capsule);

            var aabb = new c2AABB {
                min = new Vector2(300, 150),
                max = new Vector2(400, 250)
            };
            shapes.Add(aabb);

            var circle2 = new c2Circle
            {
                p = new Vector2(400, 50),
                r = 25
            };
            shapes.Add(circle2);

            man = new c2Manifold();
        }

        public void Update()
        {
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
            vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);

            var movable = shapes[0] as c2Circle;
             if (Input.IsKeyDown(Key.UP)) movable.p.Y -= 10;
             if (Input.IsKeyDown(Key.DOWN)) movable.p.Y += 10;
             if (Input.IsKeyDown(Key.LEFT)) movable.p.X -= 10;
             if (Input.IsKeyDown(Key.RIGHT)) movable.p.X += 10;
            //movable.p = platform.MousePosition;

            for (int i = 1; i < shapes.Count; i++)
            {
                var s = shapes[i];
                
                c2Collide(s, c2x.Identity, movable, c2x.Identity, man);
                if(man.count > 0)
                {
                    var n = man.normal;
                    var d = man.depths1;
                    //var sep_vec = n*d;
                    var origin = s.Center - movable.Center;
                    var sign = Vector2.Dot(n, origin) > 0 ? -1 : 1;
                    movable.p += (sign * n * d);
                }

                if (man.count > 0) vg.StrokeColor("#f00");
                else vg.StrokeColor("#fff");
                DrawShape(s);
                DrawManifold(man);
            }

            vg.StrokeColor("#00f");
            DrawShape(movable);

            vg.EndFrame();
        }

        public void DrawShape(c2Shape shape)
        {
            switch (shape)
            {
                case c2Circle c:
                    vg.DrawCircle(c.p, c.r, DrawMode.Line);
                    break;
                case c2Capsule ca:
                    var v = Vector2.Normalize(ca.b - ca.a);
                    v = new Vector2(v.Y, -v.X) * ca.r;
                    vg.DrawCircle(ca.a, ca.r, DrawMode.Line);
                    vg.DrawCircle(ca.b, ca.r, DrawMode.Line);
                    vg.DrawLine(ca.a + v, ca.b + v, ca.b - v, ca.a - v, ca.a + v);
                    break;
                case c2AABB aabb:
                    vg.DrawRectangle(aabb.min, aabb.max - aabb.min, DrawMode.Line);
                    break;
            }
        }



        public void DrawManifold(c2Manifold m)
        {
            var n = m.normal;
            vg.StrokeColor("#0f0");
            vg.FillColor("#0f0");
            for (int i = 0; i < m.count; ++i)
            {
                var p = i == 0 ? m.contact_points1 : m.contact_points2;
                float d = i == 0 ? m.depths1 : m.depths2;
                vg.DrawCircle(p, 3.0f);
                vg.DrawLine(p.X, p.Y, p.X - n.X * d, p.Y - n.Y * d);
            }
        }
    }
}

