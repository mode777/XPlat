using System.Numerics;
using NanoVGDotNet;

namespace net6test.NanoGuiPort
{
    public class BoxLayout : Layout
    {
        private Orientation horizontal;
        private Alignment middle;
        private int v1;
        private int v2;

        public BoxLayout(Orientation horizontal, Alignment middle, int v1, int v2)
        {
            this.horizontal = horizontal;
            this.middle = middle;
            this.v1 = v1;
            this.v2 = v2;
        }

        public override void PerformLayout(NVGcontext ctx, Widget widget)
        {
            throw new NotImplementedException();
        }

        public override Vector2 PreferredSize(NVGcontext ctx, Widget widget)
        {
            throw new NotImplementedException();
        }
    }
}