using System.Numerics;
using XPlat.Core;

namespace XPlat.Graphics
{
    public enum LightId {
        Light_0 = 0,
        Light_1 = 1,
        Light_2 = 2,
        Light_3 = 3,
    }

    public class PointLight
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Color { get; set; } = Vector3.One;
        public float Range { get; set; } = 10;
        public float Intensity { get; set; } = 1;

        public void ApplyToShader(Shader shader, LightId light){
            SetUniforms(shader, light, Position);
        }
        public void ApplyToShader(Shader shader, LightId light, ref Matrix4x4 mat){
            var pos = Vector3.Transform(Position, mat);
            SetUniforms(shader, light, pos);
        }

        private void SetUniforms(Shader shader, LightId light, Vector3 pos){
            shader.SetUniform(Uniform.PointLight_Pos_0.Offset((int)light), pos);
            shader.SetUniform(Uniform.PointLight_Col_0.Offset((int)light), Color);
            shader.SetUniform(Uniform.PointLight_Range_0.Offset((int)light), Range);
            shader.SetUniform(Uniform.PointLight_Int_0.Offset((int)light), Intensity);
        }

    }
}