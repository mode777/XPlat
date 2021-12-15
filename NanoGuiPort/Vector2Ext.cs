using System.Numerics;

namespace net6test.NanoGuiPort
{
    public static class Vector2Ext {
        public static Vector2 Max(Vector2 a, Vector2 b){
            return new Vector2(MathF.Max(a.X,b.X), MathF.Max(a.Y, b.Y));
        }

        public static Vector2 Min(Vector2 a, Vector2 b){
            return new Vector2(MathF.Min(a.X,b.X), MathF.Min(a.Y, b.Y));
        }

    }
}