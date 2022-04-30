using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Graphics;

namespace XPlat.SampleHost
{

    public class SpriteBufferApp : ISdlApp
    {
        private SpriteBatch batch;
        private Texture texture;
        private Transform2d transform;
        private SpriteBuffer buffer;
        private readonly IPlatform platform;

        public SpriteBufferApp(IPlatform platform)
        {
            this.platform = platform;
        }

        private float r => Random.Shared.NextSingle();

        public void Init()
        {
            batch = new SpriteBatch();
            texture = new Texture("assets/sprites/coin.png", TextureUsage.Graphics2d);
            buffer = new SpriteBuffer(texture, SpriteBatch.MAX_SPRITES);
            transform = new Transform2d();

            for (int i = 0; i < buffer.Capacity; i++)
            {
                buffer.Add();
                Reset(i);
            }

        }

        private void Reset(int i){
            buffer.Set(i,null, r * Window.Width, r * Window.Height, r * MathF.PI * 2, 0.1f, 0.1f, texture.Width/2, texture.Height/2);
        }

        public void Update()
        {
            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT);

            transform.OriginX = Window.Width / 2;
            transform.OriginY = Window.Height /2;
            transform.X = Window.Width / 2;
            transform.Y = Window.Height / 2;
            transform.Rotation += 0.01f;
            transform.ScaleX = (MathF.Sin(Time.RunningTime) + 1.5f) * 2;
            transform.ScaleY = (MathF.Sin(Time.RunningTime) + 1.5f) * 2;
            var mat = transform.GetMatrix4x4();
            //mat = Matrix4x4.Identity;

            for (int i = 0; i < buffer.Count; i++)
            {
                var x = buffer.GetX(i);
                var y = buffer.GetY(i);
                if(x < 0 || x > Window.Width || y < 0 || y > Window.Height) Reset(i);
                else {
                    x = i % 5 - 2;
                    y = i % 4 - 2;
                    buffer.Move(i, x, y);
                }
            }
            
            batch.Begin(Window.Width, Window.Height);
            batch.Draw(buffer, ref mat);
            batch.End();
        }
    }
}