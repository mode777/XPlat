using System;
using System.Numerics;
using NanoVGDotNet;

namespace net6test.NanoGuiPort
{
    public abstract class Layout
    {
        public abstract Vector2 PreferredSize(NVGcontext ctx, Widget widget);
        
        public abstract void PerformLayout(NVGcontext ctx, Widget widget);
    }
}