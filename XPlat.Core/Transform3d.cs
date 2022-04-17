using System.Numerics;

namespace XPlat.Core
{

    public class Transform3d
    {
        public Vector3 Forward => Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, RotationQuat));
        public Vector3 Right => Vector3.Normalize(Vector3.Transform(Vector3.UnitX, RotationQuat));
        public Vector3 Up => Vector3.Normalize(Vector3.Transform(Vector3.UnitY, RotationQuat));

        public Vector3 Translation;
        public Vector3 Scale;
        public Quaternion RotationQuat;

        public Vector3 RotationDeg {
            //set => RotationQuat = Quaternion.CreateFromYawPitchRoll(value.Y.ToRad(), value.X.ToRad(), value.Z.ToRad());
            get {
                float yaw,pitch,roll;
                RotationQuat.GetYawPitchRoll(out yaw, out pitch, out roll);
                return new Vector3(pitch.ToDeg(), yaw.ToDeg(), roll.ToDeg());
            }
        }

        public void RotateDeg(float x, float y, float z)
        {
            RotationQuat = Quaternion.CreateFromYawPitchRoll(y.ToRad(), x.ToRad(), z.ToRad()) * RotationQuat;
        }

        public void RotateDegLocal(float x, float y, float z)
        {
            RotationQuat = RotationQuat * Quaternion.CreateFromYawPitchRoll(y.ToRad(), x.ToRad(), z.ToRad());
        }

        public void SetRotationDeg(float x, float y, float z){
            RotationQuat = Quaternion.CreateFromYawPitchRoll(y.ToRad(), x.ToRad(), z.ToRad());
        }

        public void RotateDeg(Vector3 v) => RotateDeg(v.X, v.Y, v.Z);
        public void RotateDegLocal(Vector3 v) => RotateDegLocal(v.X, v.Y, v.Z);
        public void SetRotationDeg(Vector3 v) => SetRotationDeg(v.X, v.Y, v.Z);

        public Transform3d()
        {
            Translation = Vector3.Zero;
            RotationQuat = Quaternion.Identity;
            Scale = Vector3.One;
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

        public Transform3d(Transform3d t){
            Translation = t.Translation;
            Scale = t.Scale;
            RotationQuat = t.RotationQuat;
        }

        public Matrix4x4 GetMatrix()
        {
            return Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateFromQuaternion(RotationQuat) * Matrix4x4.CreateTranslation(Translation);
        }

        public Matrix4x4 GetNormalMatrix(){
            Matrix4x4 inv;
            Matrix4x4.Invert(Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateFromQuaternion(RotationQuat), out inv);
            return Matrix4x4.Transpose(inv);
        }
    }
}