using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Graphics;

namespace XPlat.SampleHost
{
    public class Particle
    {
        static float Ran => Random.Shared.NextSingle();

        public Particle(Texture texture, Vector2 pos)
        {
            Texture = texture;
            Trajectory = new Vector2((Ran-0.5f) * 10, (Ran-0.5f) * 10);
            Rotation = (Ran - 0.5f) * 20;
            Transform.OriginX = texture.Width / 2;
            Transform.OriginY = texture.Height / 2;
            Transform.X = pos.X;
            Transform.Y = pos.Y;
            var s = (Ran + 0.5f) * 0.3f;
            Transform.ScaleX = s;
            Transform.ScaleY = s;
        }
        public Texture Texture { get; set; }
        public Vector2 Trajectory;
        public float Rotation;
        private Matrix3x2 _mat;
        public Transform2d Transform { get; } = new Transform2d();

        public void Update()
        {
            Transform.RotationDeg += Rotation;
            Transform.X += Trajectory.X;
            Transform.Y += Trajectory.Y;
            Transform.ScaleX += 0.001f;
            Transform.ScaleY += 0.001f;
        }

        public void Draw(SpriteBatch b)
        {
            _mat = Transform.GetMatrix();
            b.SetTexture(Texture);
            b.Draw(ref _mat);
        }
    }

    public class SpriteBatchApp : ISdlApp
    {
        private SpriteBatch batch;
        private Texture texture;
        private Matrix3x2 mat;
        private Transform2d transform;
        private Particle[] particles;
        private readonly IPlatform platform;

        public SpriteBatchApp(IPlatform platform)
        {
            this.platform = platform;
        }

        public void Init()
        {
            batch = new SpriteBatch();
            texture = new Texture("assets/sprites/coin.png", TextureUsage.Graphics2d);
            mat = Matrix3x2.Identity;
            transform = new Transform2d();
            particles = Enumerable.Repeat(0, 4096).Select(x => new Particle(texture, platform.RendererSize/2)).ToArray();

            transform.OriginX = texture.Width / (float)2;
            transform.OriginY = texture.Height / (float)2;
            transform.ScaleX = 0.5f;
            transform.ScaleY = 0.5f;
        }

        public void Update()
        {
            transform.Rotation += 0.01f;
            transform.X = platform.RendererSize.X / 2;
            transform.Y = platform.RendererSize.Y / 2;
            mat = transform.GetMatrix();

            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT);

            
            batch.Begin((int)platform.RendererSize.X, (int)platform.RendererSize.Y);
            foreach (var p in particles)
            {
                p.Update();
                p.Draw(batch);
            }
            batch.End();
        }
    }
}