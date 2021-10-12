using System.Numerics;

namespace net6test
{
    public class PbrMaterial : Material
    {
        public Vector3 BaseColor { get; set; }
        public float MetallicFactor { get; set; }
        public float RoughnessFactor { get; set; }

        public override void ApplyToShader(Shader shader)
        {
            shader.SetUniform("albedo", BaseColor);
            shader.SetUniform("metallic", MetallicFactor);
            shader.SetUniform("roughness", RoughnessFactor);
        }
        
    }
}