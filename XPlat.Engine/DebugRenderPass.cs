using XPlat.Core;
using XPlat.Engine.Components;

namespace XPlat.Engine
{
    public class DebugRenderPass : IRenderPass
    {
        public IPlatform Platform { get; }
        public DebugRenderPass(IPlatform platform)
        {
            this.Platform = platform;

        }

        public void FinishFrame()
        {
            throw new NotImplementedException();
        }

        public void OnRender(Node n)
        {
            var s = n.GetComponent<Collider2dComponent>();
            if (s != null)
            {
                batch.SetSprite(s.Sprite);
                batch.Draw(ref n._globalMatrix);
            }
        }

        public void StartFrame()
        {
            throw new NotImplementedException();
        }
    }
}