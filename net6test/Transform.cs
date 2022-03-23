using System.Numerics;

namespace net6test
{
    public class Transform
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
        public Quaternion Rotation
        {
            get => _rotation; 
            set
            {
                _rotation = value;
                _rotationMatrix = Matrix4x4.CreateFromQuaternion(value);
            }
        }

        public Transform()
        {
            Translation = new Vector3();
            Rotation = new Quaternion();
            Scale = new Vector3(1,1,1);
        }

        public Matrix4x4 GetMatrix()
        {
            return _scaleMatrix * _rotationMatrix * _translationMatrix;
        }
    }
}