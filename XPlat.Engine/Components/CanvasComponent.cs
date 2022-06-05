using System.Drawing;
using System.Numerics;
using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.NanoVg;

namespace XPlat.Engine.Components
{
    [SceneElement("canvas")]
    public class CanvasComponent : Component
    {
        public event EventHandler<NVGcontext>? OnDraw;
        internal void Invoke(NVGcontext vg) => OnDraw?.Invoke(this, vg);

        public override void Parse(XElement el, SceneReader reader)
        {
            base.Parse(el, reader);
        }

        
    }
}

