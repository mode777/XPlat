// See https://aka.ms/new-console-template for more information
using GLES2;
using XPlat.Core;
using XPlat.NanoVg;

public class NanoVgApp : ISdlApp
{
    private readonly IPlatform platform;
    private NVGcontext vg;

    public NanoVgApp(IPlatform platform)
    {
        this.platform = platform;
    }

    public void Init()
    {
        this.vg = new NVGcontext();
        GlNanoVG.nvgCreateGL(ref vg, (int)NVGcreateFlags.NVG_ANTIALIAS |
                        (int)NVGcreateFlags.NVG_STENCIL_STROKES);
    }

    public void Update()
    {
        GL.ClearColor(1, 0, 0, 1);
        GL.Clear(GL.COLOR_BUFFER_BIT);

        vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);
        vg.RoundedRect(10, 10, platform.WindowSize.X - 20, platform.WindowSize.Y - 20, 10);
        vg.FillColor("#ffff00");
        vg.Fill();
        vg.EndFrame();
    }
}