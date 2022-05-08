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

        public void OnAttach(Scene scene)
        {
        }

        public void OnRender(Node n)
        {
            foreach (var c in n.Components)
            {
                switch (c)
                {
                    case SpriteComponent s:
                        batch.SetSprite(s.Sprite);
                        if(s.Origin != Vector2.Zero){
                            var mat = Matrix4x4.CreateTranslation(new Vector3(-s.Origin, 0)) * n._globalMatrix;
                            batch.Draw(ref mat);
                        } else {
                            batch.Draw(ref n._globalMatrix);
                        }
                        break;
                    case Camera2dComponent cam:
                        batch.Camera = cam.Camera;
                        cam.Camera.Transformation = n._globalMatrix;
                        break;
                    case SpriteBufferComponent b:
                        if(b.Origin != Vector2.Zero){
                            var mat = Matrix4x4.CreateTranslation(new Vector3(-b.Origin, 0)) * n._globalMatrix;
                            batch.Draw(b.Buffer, ref mat);
                        } else {
                            batch.Draw(b.Buffer, ref n._globalMatrix);
                        }
                        break;
                }

            }
        }

        public void StartFrame()
        {
            batch.Begin((int)platform.WindowSize.X, (int)platform.WindowSize.Y);
        }
    }
}