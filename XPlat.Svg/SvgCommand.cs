using System.Numerics;
using XPlat.NanoVg;

public interface ISvgCommand
{
    void Execute(NVGcontext vg);
}

public class MoveTo : ISvgCommand
{
    private readonly Vector2 point;

    public MoveTo(Vector2 point)
    {
        this.point = point;
    }
    public void Execute(NVGcontext vg)
    {
        vg.MoveTo(point.X, point.Y);
    }
}

public class LineTo : ISvgCommand
{
    private readonly Vector2 point;

    public LineTo(Vector2 point)
    {
        this.point = point;
    }
    public void Execute(NVGcontext vg)
    {
        vg.LineTo(point.X, point.Y);
    }
}

public class ArcTo : ISvgCommand
{
    private readonly Vector2 center;
    private readonly float radius;
    private readonly Vector2 a;
    private readonly int dir;

    public ArcTo(Vector2 center, float radius, Vector2 a, int dir)
    {
        this.center = center;
        this.radius = radius;
        this.a = a;
        this.dir = dir;
    }
    public void Execute(NVGcontext vg)
    {
        vg.Arc(center.X, center.Y, radius, a.X, a.Y, dir);
    }
}

public class QuadTo : ISvgCommand
{
    private readonly Vector2 control;
    private readonly Vector2 point;

    public QuadTo(Vector2 control, Vector2 point)
    {
        this.control = control;
        this.point = point;
    }

    public void Execute(NVGcontext vg)
    {
        vg.QuadTo(control.X, control.Y, point.X, point.Y);
    }
}

public class CubicTo : ISvgCommand
{
    private readonly Vector2 control1;
    private readonly Vector2 control2;
    private readonly Vector2 point;

    public CubicTo(Vector2 control1, Vector2 control2, Vector2 point)
    {
        this.control1 = control1;
        this.control2 = control2;
        this.point = point;
    }

    public void Execute(NVGcontext vg)
    {
        vg.BezierTo(control1.X, control1.Y, control2.X, control2.Y, point.X, point.Y);
    }
}

public class ClosePath : ISvgCommand
{
    public void Execute(NVGcontext vg)
    {
        vg.ClosePath();
    }
}