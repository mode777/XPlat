using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Graphics;

namespace XPlat.Engine
{

    public class RenderPass2d : IRenderPass
    {
        private readonly IPlatform platform;
        private SpriteBatch batch;

        public RenderPass2d(IPlatform platform)
        {
            this.platform = platform;
            this.batch = new SpriteBatch();
        }

        public void FinishFrame()
        {
            batch.End();
        }

        public void OnRender(Node n)
        {
            var s = n.GetComponent<SpriteComponent>();
            if(s != null)
            {
                batch.SetSprite(s.Sprite);
                batch.Draw(ref n._globalMatrix);
            }
        }

        public void StartFrame()
        {
            batch.Begin((int)platform.WindowSize.X, (int)platform.WindowSize.Y);
        }
    }
}