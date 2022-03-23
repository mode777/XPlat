using System.Numerics;

namespace XPlat.Core
{

    public class Transform3d
    {
        Matrix4x4 _scaleMatrix;
        Matrix4x4 _translationMatrix;
        Matrix4x4 _rotationMatrix;
        private Vector3 _translation;
        private Vector3 _scale;
        private Quaternion _rotation;

        public Vector3 Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                _translationMatrix = Matrix4x4.CreateTranslation(value);
            }
        }
        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _scaleMatrix = Matrix4x4.CreateScale(value);
            }
        }
        public Quaternion RotationQuat
        {
            get => _rotation; 
            set
            {
                _rotation = value;
                _rotationMatrix = Matrix4x4.CreateFromQuaternion(value);
                
            }
        }

        public Vector3 RotationDeg {
            set => RotationQuat = Quaternion.CreateFromYawPitchRoll(value.Y.ToRad(), value.X.ToRad(), value.Z.ToRad());
            get {
                float yaw,pitch,roll;
                _rotation.GetYawPitchRoll(out yaw, out pitch, out roll);
                return new Vector3(pitch.ToDeg(), yaw.ToDeg(), roll.ToDeg());
            }
        }

        public Transform3d()
        {
            Translation = new Vector3();
            RotationQuat = new Quaternion();
            Scale = new Vector3(1,1,1);
        }

        public Matrix4x4 GetMatrix()
        {
            return _scaleMatrix * _rotationMatrix * _translationMatrix;
        }

        public Matrix4x4 GetNormalMatrix(){
            Matrix4x4 inv;
            Matrix4x4.Invert(_scaleMatrix * _rotationMatrix, out inv);
            return Matrix4x4.Transpose(inv);
        }
    }
}