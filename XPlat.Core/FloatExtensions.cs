using System;

namespace XPlat.Core
{
    public static class FloatExtensions {
        public static float ToRad(this float f) => f * (MathF.PI/180);
        public static float ToDeg(this float f) => f * (180 / MathF.PI);
    } 

}
