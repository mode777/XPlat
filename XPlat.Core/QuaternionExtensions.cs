using System;
using System.Numerics;

namespace XPlat.Core
{
    public static class QuaternionExtensions {
        public static void GetYawPitchRoll(this Quaternion q, out float yaw, out float pitch, out float roll){
            pitch = MathF.Atan2(2*(q.Y*q.Z + q.W*q.X), q.W*q.W - q.X*q.X - q.Y*q.Y + q.Z*q.Z);
            yaw = MathF.Asin(-2*(q.X*q.Z - q.W*q.Y));
            roll = MathF.Atan2(2*(q.X*q.Y + q.W*q.Z), q.W*q.W + q.X*q.X - q.Y*q.Y - q.Z*q.Z);
        }
    }
}