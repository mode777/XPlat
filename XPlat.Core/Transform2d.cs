using System;
using System.Numerics;

namespace XPlat.Core {

    public class Transform2d
    {
        private float _rotation = 0;
        private float _sin = 0;
        private float _cos = 1;
        
        public Transform2d()
        {
        }
        
        public float X { get; set; }

        public float Y { get; set; }
        public float OriginX { get; set; }

        public float OriginY { get; set; }

        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _cos = MathF.Cos(value);
                _sin = MathF.Sin(value);
            }
        }

        public float RotationDeg
        {
            get => Rotation.ToDeg(); 
            set => Rotation = value.ToRad();
        }

        public float ScaleX { get; set; } = 1;
        public float ScaleY { get; set; } = 1;

        public Matrix3x2 GetMatrix()
        {
            var matrix = new Matrix3x2();
            matrix.M11 = ScaleX * _cos;
            matrix.M12 = ScaleX * _sin;

            matrix.M21 = ScaleY * -_sin;
            matrix.M22 = ScaleY * _cos;

            matrix.M31 = -OriginX * ScaleX * _cos + -OriginY * ScaleY * -_sin + X;
            matrix.M32 = -OriginX * ScaleX * _sin + -OriginY * ScaleY * _cos + Y;
            return matrix;
        }

        public Matrix4x4 GetMatrix4x4(){
            return new Matrix4x4(GetMatrix());
        }

        public Vector2 TransformPoint(Vector2 vec){
            return Vector2.Transform(vec, GetMatrix());
        }

    }
}
