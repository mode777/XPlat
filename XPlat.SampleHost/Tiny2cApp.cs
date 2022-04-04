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
        private float r;

        public Tiny2cApp(IPlatform platform)
        {
            this.platform = platform;
        }

        public void Init()
        {
            this.vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS | NVGcreateFlags.NVG_STENCIL_STROKES);

            c2Circle c = new c2Circle();
			c.p = new Vector2(100,100);
		    c.r = 50;

            c2Capsule cap = new c2Capsule();
            cap.a = new Vector2(150,150);
			cap.b = new Vector2(300,300);
            cap.r = 50;

            var a = new c2x();
            var b = new c2x();

            var hit = c2Collided(c, c2x.Identity, cap, c2x.Identity);
        }

        public void Update()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);

            vg.EndFrame();
        }
    }
}

