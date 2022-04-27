// See https://aka.ms/new-console-template for more information
using System.Drawing;
using GLES2;
using XPlat.Core;
using XPlat.NanoVg;

public class NanoVgApp : ISdlApp
{
    private readonly IPlatform platform;
    private NVGcontext vg;
    private int img;

    public NanoVgApp(IPlatform platform)
    {
        this.platform = platform;
    }

    public void Init()
    {
        this.vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS | NVGcreateFlags.NVG_STENCIL_STROKES);
        this.img = vg.CreateImage("assets/ui/DefaultSkin2.png", 0);
    }

    int o = 0;

    public void Update()
    {
        o++;
        GL.ClearColor(1, 0, 0, 1);
        GL.Clear(GL.COLOR_BUFFER_BIT);

        vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);
        vg.BeginPath();
        vg.RoundedRect(10, 10, platform.WindowSize.X - 20, platform.WindowSize.Y - 20, 10);
        vg.FillColor("#ffff00");
        vg.Fill();


        float u1 = 32 / (float)512;
        float u2 = (32 + 25) / (float)512;
        float v1 = 225 / (float)512;
        float v2 = (225 + 25) / (float)512;

        var targetRect = new Rectangle(100,100,300-o,100);
        var RenderOffset = new Point(100,100);
        var w = 512;
        var h = 512;

        var x = targetRect.X + RenderOffset.X;
        var y = targetRect.Y + RenderOffset.Y;

        vg.BeginPath();
        vg.Rect(x,y,targetRect.Width,targetRect.Height);

        var ix = u1 * w;
        var ix2 = u2 * w;
        var iy = v1 * h;
        var iy2 = v2 * h;
        var sx = targetRect.Width / (float)(ix2 - ix);
        var sy = targetRect.Height / (float)(iy2 - iy);

        var p = vg.ImagePattern(x-(ix*sx),y-(iy*sy),w*sx,h*sy,0,img,1);
        vg.FillPaint(p);
        vg.Fill();

        vg.EndFrame();
    }
}