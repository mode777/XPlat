using System.Numerics;

namespace XPlat.Core
{

    public class Transform3d
    {
        public Vector3 Forward => Vector3.Transform(Vector3.UnitZ, RotationQuat);
        public Vector3 Right => Vector3.Transform(Vector3.UnitX, RotationQuat);
        public Vector3 Up => Vector3.Transform(Vector3.UnitY, RotationQuat);

        public Vector3 TranslationVector;
        public Vector3 ScaleVector;
        public Quaternion RotationQuat;

        public Vector3 RotationDeg {
            //set => RotationQuat = Quaternion.CreateFromYawPitchRoll(value.Y.ToRad(), value.X.ToRad(), value.Z.ToRad());
            get {
                float yaw,pitch,roll;
                RotationQuat.GetYawPitchRoll(out yaw, out pitch, out roll);
                return new Vector3(pitch.ToDeg(), yaw.ToDeg(), roll.ToDeg());
            }
        }

        public void MoveForward(float amnt) => TranslationVector += (Forward * amnt);
        public void MoveRight(float amnt) => TranslationVector += (Right * amnt);
        public void MoveUp(float amnt) => TranslationVector += (Up * amnt);
        public float X { get => TranslationVector.X; set => TranslationVector.X = value; }
        public float Y { get => TranslationVector.Y; set => TranslationVector.Y = value; }
        public float Z { get => TranslationVector.Z; set => TranslationVector.Z = value; }

        public void RotateDeg(float x, float y, float z)
        {
            RotationQuat = Quaternion.CreateFromYawPitchRoll(y.ToRad(), x.ToRad(), z.ToRad()) * RotationQuat;
        }

        public void RotateDegLocal(float x, float y, float z)
        {
            RotationQuat = RotationQuat * Quaternion.CreateFromYawPitchRoll(y.ToRad(), x.ToRad(), z.ToRad());
        }

        public void SetScale(float s){
            ScaleVector = new Vector3(s,s,s);
        }

        public void Scale(float s)
        {
            ScaleVector *= new Vector3(s, s, s);
        }

        public float GetScale() => ScaleVector.X;
        public float ScaleX => ScaleVector.X;
        public float ScaleY => ScaleVector.Y;
        public float ScaleZ => ScaleVector.Z;

        public void SetRotationDeg(float x, float y, float z){
            RotationQuat = Quaternion.CreateFromYawPitchRoll(y.ToRad(), x.ToRad(), z.ToRad());
        }
        public void Translate(float x, float y, float z) => TranslationVector += new Vector3(x, y, z);
        public void Translate(Vector3 v) => TranslationVector += v;
        public void SetTranslation(float x, float y, float z) => TranslationVector = new Vector3(x, y, z);
        public void RotateDeg(Vector3 v) => RotateDeg(v.X, v.Y, v.Z);
        public void RotateDegLocal(Vector3 v) => RotateDegLocal(v.X, v.Y, v.Z);
        public void SetRotationDeg(Vector3 v) => SetRotationDeg(v.X, v.Y, v.Z);

        public Transform3d()
        {
            TranslationVector = Vector3.Zero;
            RotationQuat = Quaternion.Identity;
            ScaleVector = Vector3.One;
        }

        public Transform3d(Matrix4x4 m){
            TranslationVector = new Vector3(m.M41, m.M42, m.M43);
            ScaleVector = new Vector3(new Vector3(m.M11, m.M12, m.M13).Length(),
                new Vector3(m.M21, m.M22, m.M23).Length(),
                new Vector3(m.M31, m.M32, m.M33).Length());
            var rotMat = new Matrix4x4(
                m.M11 / ScaleVector.X, m.M21 / ScaleVector.Y, m.M31 / ScaleVector.Z, 0,
                m.M12 / ScaleVector.X, m.M22 / ScaleVector.Y, m.M32 / ScaleVector.Z, 0,
                m.M13 / ScaleVector.X, m.M23 / ScaleVector.Y, m.M33 / ScaleVector.Z, 0,
                0, 0, 0, 1);
            RotationQuat = Quaternion.CreateFromRotationMatrix(rotMat);
        }

        public Transform3d(Transform3d t){
            TranslationVector = t.TranslationVector;
            ScaleVector = t.ScaleVector;
            RotationQuat = t.RotationQuat;
        }

        public Matrix4x4 GetMatrix()
        {
            return Matrix4x4.CreateScale(ScaleVector) * Matrix4x4.CreateFromQuaternion(RotationQuat) * Matrix4x4.CreateTranslation(TranslationVector);
        }

        public Matrix4x4 GetNormalMatrix(){
            Matrix4x4 inv;
            Matrix4x4.Invert(Matrix4x4.CreateScale(ScaleVector) * Matrix4x4.CreateFromQuaternion(RotationQuat), out inv);
            return Matrix4x4.Transpose(inv);
        }
    }
}