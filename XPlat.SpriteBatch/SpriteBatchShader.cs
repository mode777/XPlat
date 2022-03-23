using System;
using System.Collections.Generic;
using System.Numerics;
using XPlat.Core;
using XPlat.Graphics;

namespace XPlat.Graphics
{
    internal class SpriteBatchShader : Shader
    {
        public static readonly SpriteBatchShader Singleton = new SpriteBatchShader();

        private SpriteBatchShader()
            : base(
                Resource.LoadResourceString<SpriteBatchShader>("vertex.glsl"), 
                Resource.LoadResourceString<SpriteBatchShader>("fragment.glsl"), 
                new Dictionary<Attribute, string>{
                    [Attribute.Position] = "aPos",
                    [Attribute.Uv_0] = "aUv",
                    [Attribute.Color] = "aColor",
                }, 
                new Dictionary<Uniform, string>{
                    [Uniform.AlbedoTexture] = "uTexture",
                    [Uniform.ViewportSize] = "uViewportSize",
                    [Uniform.TextureSize] = "uTextureSize",
                })
        {


        }



    }
}

