using System;
using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.NanoVg;
using static TinyC2.TinyC2Api;

namespace XPlat.SampleHost
{
    public class Tiny2cApp : ISdlApp
    {
        private readonly IPlatform platform;
        private NVGcontext vg;
        private c2Circle circle;
        private float r;
        private c2Capsule capsule;
        private List<c2Shape> shapes = new List<c2Shape>();
        private c2Manifold man;

        public Tiny2cApp(IPlatform platform)
        {
            this.platform = platform;
        }

        public void Init()
        {
            this.vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS | NVGcreateFlags.NVG_STENCIL_STROKES);

            circle = new c2Circle();
            circle.p = new Vector2(100, 100);
            circle.r = 50;
            shapes.Add(circle);

            capsule = new c2Capsule {
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

            man = new c2Manifold();
        }

        public void Update()
        {
            circle.p = platform.MousePosition;
            c2Collide(circle, c2x.Identity, capsule, c2x.Identity, man);

            GL.ClearColor(0.2f, 0.2f, 0.2f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);

            if (man.count > 0) vg.StrokeColor("#f00");
            else vg.StrokeColor("#fff");
            DrawShape(circle);
            DrawShape(capsule);
            DrawManifold(man);

            vg.EndFrame();
        }

        public void DrawShape(c2Shape shape)
        {
            switch (shape)
            {
                case c2Circle c:
                    vg.DrawCircle(c.p, c.r, false, true);
                    break;
                case c2Capsule ca:
                    var v = Vector2.Normalize(ca.b - ca.a);
                    v = new Vector2(v.Y, -v.X) * ca.r;
                    vg.DrawCircle(ca.a, ca.r, false, true);
                    vg.DrawCircle(ca.b, ca.r, false, true);
                    vg.DrawLine(ca.a + v, ca.b + v, ca.b - v, ca.a - v, ca.a + v);
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
                vg.DrawCircle(p, 3.0f, true, false);
                vg.DrawLine(p.X, p.Y, p.X - n.X * d, p.Y - n.Y * d);
            }
        }
    }
}

