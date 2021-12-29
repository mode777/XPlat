using GLES2;
using System.Numerics;

namespace net6test
{

    public class LightmapMaterial : Material
    {
        public uint Texture { get; set; }

        public override void ApplyToShader(Shader shader)
        {
            GL.ActiveTexture(GL.TEXTURE0);
            GL.BindTexture(GL.TEXTURE_2D, Texture);
            shader.SetUniform(StandardUniform.LightmapTexture, 0);
        }
    }

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