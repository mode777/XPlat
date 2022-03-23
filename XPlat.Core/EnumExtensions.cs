using System;
using System.Runtime.CompilerServices;

namespace XPlat.Core {
    public static class EnumExtensions {
        public static T Offset<T>(this T e, int n) where T : Enum {
            var val = Unsafe.As<T,int>(ref e);
            val += n;
            return Unsafe.As<int,T>(ref val);
        }
    }
}