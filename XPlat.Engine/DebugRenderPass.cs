using System.Numerics;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Graphics;
using XPlat.NanoVg;
using static TinyC2.TinyC2Api;

namespace XPlat.Engine
{
    public class DebugRenderPass : IRenderPass
    {
        public IPlatform Platform { get; }
        public Scene Scene { get; private set; }
        public Camera2dComponent? Camera { get; private set; }

        private NVGcontext vg;

        public DebugRenderPass(IPlatform platform)
        {
            this.Platform = platform;
            this.vg = NVGcontext.CreateGl();
        }

        public void FinishFrame()
        {
            vg.EndFrame();
        }

        public void OnRender(Node n)
        {
            Matrix4x4 mat = n._globalMatrix * Camera.Camera.TransformationInverse;

            var s = n.GetComponent<Collider2dComponent>();
            if (s != null)
            {
                var shape = s.Shape.GetTransformed(ref mat);
                if(n.Collisions.Any()) 
                    vg.StrokeColor("#f00");
                else
                    vg.StrokeColor("#fff");

                DrawShape(shape);
                vg.StrokeColor("#0f0");
                var bbox = s.Shape.GetBBox(ref mat);
                DrawShape(bbox);
            }
        }

        private void DrawShape(c2Shape shape)
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

        public void StartFrame()
        {
            Camera = Camera ?? Scene.FindNode("camera").GetComponent<Camera2dComponent>();

            vg.BeginFrame((int)Platform.WindowSize.X, (int)Platform.WindowSize.Y, Platform.RetinaScale);
            
        }

        public void OnAttach(Scene scene)
        {
            Scene = scene;
        }
    }
}