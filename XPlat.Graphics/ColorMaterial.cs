using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Core;

namespace XPlat.Graphics
{

    public class ColorMaterial : PhongMaterial
    {

        public ColorMaterial(int r, int g, int b, int a) : base(new Texture(new Image<Rgba32>(1,1,new Rgba32(r,g,b,a))), Uniform.AlbedoTexture)
        {
        }
    }
}