using System.Numerics;

namespace XPlat.Core
{

    public class Transform3d
    {
        public Matrix4x4 ScaleMatrix;
        public Matrix4x4 TranslationMatrix;
        public Matrix4x4 RotationMatrix;
        private Vector3 _translation;
        private Vector3 _scale;
        private Quaternion _rotation;

        public Vector3 Forward => Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, RotationMatrix));
        public Vector3 Right => Vector3.Normalize(Vector3.Transform(Vector3.UnitX, RotationMatrix));

        public Vector3 Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                TranslationMatrix = Matrix4x4.CreateTranslation(value);
            }
        }
        public Vector3 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                ScaleMatrix = Matrix4x4.CreateScale(value);
            }
        }
        public Quaternion RotationQuat
        {
            get => _rotation; 
            set
            {
                _rotation = value;
                RotationMatrix = Matrix4x4.CreateFromQuaternion(value);
                
            }
        }

        public Vector3 RotationDeg {
            //set => RotationQuat = Quaternion.CreateFromYawPitchRoll(value.Y.ToRad(), value.X.ToRad(), value.Z.ToRad());
            get {
                float yaw,pitch,roll;
                _rotation.GetYawPitchRoll(out yaw, out pitch, out roll);
                return new Vector3(pitch.ToDeg(), yaw.ToDeg(), roll.ToDeg());
            }
        }

        public void RotateDeg(float x, float y, float z)
        {
            RotationQuat = Quaternion.CreateFromYawPitchRoll(y.ToRad(), x.ToRad(), z.ToRad());
        }

        public void RotateDeg(Vector3 v)
        {
            RotationQuat = Quaternion.CreateFromYawPitchRoll(v.Y.ToRad(), v.X.ToRad(), v.Z.ToRad());
        }

        public Transform3d()
        {
            Translation = new Vector3();
            RotationQuat = new Quaternion();
            Scale = new Vector3(1,1,1);
        }

        public Transform3d(Matrix4x4 m){
            Translation = new Vector3(m.M41, m.M42, m.M43);
            Scale = new Vector3(new Vector3(m.M11, m.M12, m.M13).Length(),
                new Vector3(m.M21, m.M22, m.M23).Length(),
                new Vector3(m.M31, m.M32, m.M33).Length());
            var rotMat = new Matrix4x4(
                m.M11 / Scale.X, m.M21 / Scale.Y, m.M31 / Scale.Z, 0,
                m.M12 / Scale.X, m.M22 / Scale.Y, m.M32 / Scale.Z, 0,
                m.M13 / Scale.X, m.M23 / Scale.Y, m.M33 / Scale.Z, 0,
                0, 0, 0, 1);
            RotationQuat = Quaternion.CreateFromRotationMatrix(rotMat);
        }

        public bool IsIdentity => ScaleMatrix.IsIdentity && RotationMatrix.IsIdentity && TranslationMatrix.IsIdentity;

        public Matrix4x4 GetMatrix()
        {
            return ScaleMatrix * RotationMatrix * TranslationMatrix;
        }

        public Matrix4x4 GetNormalMatrix(){
            Matrix4x4 inv;
            Matrix4x4.Invert(ScaleMatrix * RotationMatrix, out inv);
            return Matrix4x4.Transpose(inv);
        }
    }
}