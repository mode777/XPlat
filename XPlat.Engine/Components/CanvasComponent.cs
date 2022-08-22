using System.Drawing;
using System.Numerics;
using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.NanoVg;
using XPlat.WrenScripting;

namespace XPlat.Engine.Components
{
    [SceneElement("canvas")]
    public class CanvasComponent : Component
    {
        public event EventHandler<NVGcontext>? OnDraw;
        internal void Invoke(NVGcontext vg) => OnDraw?.Invoke(this, vg);
        public void RegisterWrenComponent(WrenObjectHandle handle){
            OnDraw += (o,v) => handle.Call("draw(_)", v);
        }

        public override void Parse(XElement el, SceneReader reader)
        {
            base.Parse(el, reader);
        }

        
    }
}

