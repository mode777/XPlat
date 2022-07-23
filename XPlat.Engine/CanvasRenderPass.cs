using System.Numerics;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Graphics;
using XPlat.NanoVg;
using static TinyC2.TinyC2Api;

namespace XPlat.Engine
{
    public class CanvasRenderPass : IRenderPass
    {
        public IPlatform Platform { get; }
        public Scene Scene { get; private set; }
        private NVGcontext vg;

        public NVGcontext Context => vg;

        public CanvasRenderPass(IPlatform platform, NVGcontext vg)
        {
            this.Platform = platform;
            this.vg = vg;
        }

        public void FinishFrame()
        {
            vg.EndFrame();
        }

        public void OnRender(Node n)
        {   
            var s = n.GetComponent<CanvasComponent>();
            s?.Invoke(vg);
        }
        public void StartFrame()
        {
            vg.BeginFrame((int)Platform.WindowSize.X, (int)Platform.WindowSize.Y, Platform.RetinaScale);
        }

        public void OnAttach(Scene scene)
        {
            Scene = scene;
            
        }
    }
}