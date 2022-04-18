using System.Numerics;
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
            foreach (var c in n.Components)
            {
                if(c is SpriteComponent s){
                    batch.SetSprite(s.Sprite);
                    if(s.Origin != Vector2.Zero){
                        var mat = Matrix4x4.CreateTranslation(new Vector3(-s.Origin, 0)) * n._globalMatrix;
                        batch.Draw(ref mat);
                    } else {
                        batch.Draw(ref n._globalMatrix);
                    }
                }
                if(c is Camera2dComponent cam){
                    batch.Camera = cam.Camera;
                    cam.Camera.Transformation = n._globalMatrix;
                }
            }
        }

        public void StartFrame()
        {
            batch.Begin((int)platform.WindowSize.X, (int)platform.WindowSize.Y);
        }
    }
}