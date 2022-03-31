using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Graphics;

namespace XPlat.SampleHost
{
    public class SpriteBatchApp : ISdlApp
    {
        private SpriteBatch batch;
        private Texture texture;
        private Texture texture2;
        private Matrix3x2 mat;
        private Transform2d transform;
        private readonly IPlatform platform;

        public SpriteBatchApp(IPlatform platform)
        {
            this.platform = platform;
        }

        public void Init()
        {
            this.batch = new SpriteBatch(16);
            this.texture = new Texture("assets/desktop.jpeg", TextureUsage.Graphics2d);
            this.texture2 = new Texture("assets/ijon.jpeg", TextureUsage.Graphics2d);
            this.mat = Matrix3x2.Identity;
            this.transform = new Transform2d();

            transform.OriginX = texture.Size.X / 2;
            transform.OriginY = texture.Size.Y / 2;
            transform.ScaleX = 0.5f;
            transform.ScaleY = 0.5f;
        }

        public void Update()
        {
            transform.Rotation += 0.01f;
            transform.X = platform.RendererSize.X / 2;
            transform.Y = platform.RendererSize.Y / 2;
            transform.GetMatrix(ref mat);

            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT);
            
            batch.Begin((int)platform.RendererSize.X, (int)platform.RendererSize.Y);
            batch.SetTexture(texture);
            batch.Draw(0,0);
            batch.SetColor(new Color(0,0,255));
            batch.Draw(ref mat);
            batch.SetColor(new Color(255,255,255));
            batch.SetTexture(texture2);
            batch.Draw(400,400);
            batch.End();
        }
    }
}