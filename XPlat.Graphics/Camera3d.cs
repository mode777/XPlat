using System;
using System.Numerics;
using XPlat.Core;

namespace XPlat.Graphics
{

    public class Camera3d : Camera
    {
        private Matrix4x4 matProj;
        private Matrix4x4 matView;

        public Vector3 Positon = Vector3.Zero;
        public Vector3 Target = -Vector3.UnitZ;
        public float Ratio = 16 / 10;
        public float Fov = 60 * (MathF.PI / 180);
        public float FovDeg 
        { 
            get => Fov * (180 / MathF.PI); 
            set => Fov = value * (MathF.PI / 180); 
        }

        public float NearPlane = 0.1f;
        public float FarPlane = 100;

        public void ApplyToShader(Shader shader, ref Matrix4x4 transform)
        {
            var position = Vector3.Transform(Positon, transform);
            var target = Vector3.Transform(Target, transform);  
            //var up = Vector3.Transform(Vector3.UnitY, transform);

            UpdateMatrix(position, target, Vector3.UnitY);
            Shader.Use(shader);
            shader.SetUniform(Uniform.ProjectionMatrix, ref matProj);
            shader.SetUniform(Uniform.ViewMatrix, ref matView);
            shader.SetUniform(Uniform.CameraPositon, position);
        }

        public async void ApplyToShader(Shader shader)
        {
            UpdateMatrix(Positon, Target, Vector3.UnitY);
            Shader.Use(shader);
            shader.SetUniform(Uniform.ProjectionMatrix, ref matProj);
            shader.SetUniform(Uniform.ViewMatrix, ref matView);
            shader.SetUniform(Uniform.CameraPositon, Positon);
        }

        private void UpdateMatrix(Vector3 position, Vector3 target, Vector3 up)
        {
            matView = Matrix4x4.CreateLookAt(position, target, up);
            matProj = Matrix4x4.CreatePerspectiveFieldOfView(Fov, Ratio, NearPlane, FarPlane);
        }
    }
}