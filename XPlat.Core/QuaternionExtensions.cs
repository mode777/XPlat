using System;
using System.Numerics;

namespace XPlat.Core
{
    public static class QuaternionExtensions {
        public static void GetYawPitchRoll(this Quaternion q, out float yaw, out float pitch, out float roll){
            // pitch (x-axis rotation)
            float sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            float cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            pitch = MathF.Atan2(sinr_cosp, cosr_cosp);

            // yaw (y-axis rotation)
            float sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (MathF.Abs(sinp) >= 1)
                yaw = MathF.PI / 2 * Math.Sign(sinp); // use 90 degrees if out of range
            else
                yaw = MathF.Asin(sinp);

            // roll (z-axis rotation)
            float siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            float cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            roll = MathF.Atan2(siny_cosp, cosy_cosp);

        }
    }
}