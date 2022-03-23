// See https://aka.ms/new-console-template for more information
using GLES2;
using XPlat.Core;
using XPlat.NanoVg;
using XPlat.Svg;

public class SvgApp : ISdlApp
{
    private readonly IPlatform platform;
    private NVGcontext vg;
    private SvgImage svg;

    public SvgApp(IPlatform platform)
    {
        this.platform = platform;
    }

    public void Init()
    {
        this.vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS |
                        NVGcreateFlags.NVG_STENCIL_STROKES);

        this.svg = SvgImage.Load("assets/Ghostscript_Tiger.svg");
    }

    public void Update()
    {
        GL.ClearColor(1, 0, 0, 1);
        GL.Clear(GL.COLOR_BUFFER_BIT);

        vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);
        vg.DrawSvg(svg);
        vg.EndFrame();
    }
}