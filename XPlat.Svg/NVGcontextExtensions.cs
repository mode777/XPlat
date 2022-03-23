using XPlat.NanoVg;

namespace XPlat.Svg;
public static class NVGcontextExtensions
{
    public static void DrawSvg(this NVGcontext vg, SvgImage image){
        image.Document.Draw(vg);
    }

}
