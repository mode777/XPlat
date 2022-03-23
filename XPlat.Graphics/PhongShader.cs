using System.Collections.Generic;
using XPlat.Core;

namespace XPlat.Graphics
{
    public class PhongShader : Shader
    {
        public PhongShader()
            : base(
                Resource.LoadResourceString<PhongShader>("phong.vertex.glsl"), 
                Resource.LoadResourceString<PhongShader>("phong.fragment.glsl"),
                new Dictionary<Attribute, string>
                {
                    [Attribute.Position] = "aPos",
                    [Attribute.Normal] = "aNormal",
                    [Attribute.Uv_0] = "aUv",
                },
                new Dictionary<Uniform, string>
                {
                    [Uniform.ModelMatrix] = "uModel",
                    [Uniform.ViewMatrix] = "uView",
                    [Uniform.ProjectionMatrix] = "uProjection",
                    [Uniform.NormalMatrix] = "uNormal",
                    [Uniform.AlbedoTexture] = "uTexture",
                    [Uniform.CameraPositon] = "uViewPos",
                    [Uniform.PointLight_Pos_0] = "uPointLights[0].position",
                    [Uniform.PointLight_Col_0] = "uPointLights[0].color",
                    [Uniform.PointLight_Range_0] = "uPointLights[0].range",
                    [Uniform.PointLight_Int_0] = "uPointLights[0].intensity",
                    [Uniform.PointLight_Pos_1] = "uPointLights[1].position",
                    [Uniform.PointLight_Col_1] = "uPointLights[1].color",
                    [Uniform.PointLight_Range_1] = "uPointLights[1].range",
                    [Uniform.PointLight_Int_1] = "uPointLights[1].intensity",
                })
        {
            
        }
    }
}