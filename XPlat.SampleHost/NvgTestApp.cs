// See https://aka.ms/new-console-template for more information
using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.NanoVg;

public class NvgTestApp : ISdlApp
{
    private readonly IPlatform platform;
    private NVGcontext vg;
    private Transform2d transform;
    private Matrix3x2 mat;
    private float r;

    public NvgTestApp(IPlatform platform)
    {
        this.platform = platform;
    }

    public void Init()
    {
        this.vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS |
                        NVGcreateFlags.NVG_STENCIL_STROKES);

        this.transform = new Transform2d();
    }

    public void Update()
    {
        GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
        GL.Clear(GL.COLOR_BUFFER_BIT);

        vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);

        vg.StrokeColor("#000000");
        vg.StrokeWidth(15);

        transform.RotationDeg = 0;
        var o = new Vector2(300,300);
        var rad = 200;
        var p = o + new Vector2(rad, 0);
        var n = 10;

        vg.BeginPath();
        vg.MoveTo(p.X, p.Y);
        for (int i = 0; i < (n-1); i++)
        {
            transform.RotationDeg += 360 / n;
            p = o + transform.TransformPoint(new Vector2(rad,0));
            vg.LineTo(p.X, p.Y);
        }
        vg.ClosePath();
        vg.Stroke();

        vg.EndFrame();
    }
}