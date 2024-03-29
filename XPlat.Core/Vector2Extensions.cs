﻿using System;
using System.Numerics;

namespace XPlat.Core
{
    public static class Vector2Extensions
    {
        public static float Component(this Vector2 v, int i) => i == 0 ? v.X : v.Y;
        public static float Component(this ref Vector2 v, int i, float value) => i == 0 ? v.X = value : v.Y = value;
        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            return new Vector2(MathF.Max(a.X, b.X), MathF.Max(a.Y, b.Y));
        }

        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            return new Vector2(MathF.Min(a.X, b.X), MathF.Min(a.Y, b.Y));
        }
    }

}
