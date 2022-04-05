

using System;
using System.IO;
using System.Linq;
using System.Numerics;

namespace XPlat.NanoVg
{
    public static class ContextExtensions
    {
        public static void DrawCircle(this NVGcontext vg, Vector2 p, float r, bool fill, bool stroke){
            vg.BeginPath();
            vg.Circle(p.X, p.Y, r);
            if(fill) vg.Fill();
            if(stroke) vg.Stroke();
        }



        public static async void DrawLine(this NVGcontext vg, params Vector2[] vs){
            vg.BeginPath();
            var f = vs.First();
            vg.MoveTo(f.X, f.Y);
            foreach (var v in vs.Skip(1))
            {
                vg.LineTo(v.X, v.Y);
            }
            vg.Stroke();
        }

        public static async void DrawLine(this NVGcontext vg, params float[] vs){
            vg.BeginPath();
            var x = vs[0];
            var y = vs[1];
            vg.MoveTo(x, y);
            for (int i = 2; i < vs.Length; i+=2)
            {
                vg.LineTo(vs[i], vs[i+1]);
            }
            vg.Stroke();
        }
    }
}