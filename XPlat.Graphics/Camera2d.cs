using System.Numerics;
using XPlat.Core;

namespace XPlat.Graphics
{
    public class Camera2d : Camera
    {
        public Matrix4x4 ViewMatrix = Matrix4x4.CreateTranslation(new Vector3(-1,-1,0)) * Matrix4x4.CreateScale(1,-1,1);
        public Matrix4x4 ProjectionMatrix;
        public Matrix4x4 ViewProjection;
        public Vector2 Size {
            get => _size;
            set {
                _size = value;
                ProjectionMatrix = Matrix4x4.CreateOrthographic(Size.X, Size.Y, NearPlane, FarPlane);
                _post = Matrix4x4.CreateTranslation(Size.X / 2, Size.Y / 2, 0);
                _pre = Matrix4x4.CreateTranslation(-Size.X / 2, -Size.Y / 2, 0);
            }
        }
        public float NearPlane = 0.0f;
        public float FarPlane = 100;
        public Matrix4x4 Transformation = Matrix4x4.Identity;
        private Vector2 _size;
        private Matrix4x4 _pre;
        private Matrix4x4 _post;

        public void Update()
        {
            if(Transformation == Matrix4x4.Identity){
                ViewProjection = ProjectionMatrix * ViewMatrix;
            } else {
                Matrix4x4 m;
                Matrix4x4.Invert(Transformation, out m);
                ViewProjection =  _pre * m * _post * ProjectionMatrix * ViewMatrix;
            }
        }

        public void ApplyToShader(Shader shader)
        {
            Update();
            shader.SetUniform(Uniform.ViewProjectionMatrix, ref ViewProjection);
        }
    }
}