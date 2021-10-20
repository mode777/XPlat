using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NanoVGDotNet;

namespace net6test.UI
{
    public enum Unit
    {
        Point,
        ViewportWidth,
        ViewportHeight
    }

    public struct ShadowDrawParams
    {
        public RectangleF Rect;
        public NVGpaint Paint;

        public ShadowDrawParams(NVGcontext vg, RectangleF caster, float casterCorner, PointF offset, float size, NVGcolor color)
        {
            Rect = caster.Expand(size);
            Rect.Offset(offset);

            var trans = color;
            trans.a = 0;
            Paint = vg.BoxGradient(
                x: caster.X,
                y: caster.Y,
                w: caster.Width,
                h: caster.Height,
                r: (size / 2f) + casterCorner,
                f: size,
                icol: color,
                ocol: trans);
        }
                        
        public void Draw(NVGcontext vg, RectangleF caster, float cornerRadius)
        {
            vg.BeginPath();
            vg.Rect(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            vg.PathWinding((int)NVGsolidity.NVG_HOLE);
            vg.RoundedRect(caster.X, caster.Y, caster.Width, caster.Height, cornerRadius);
            vg.FillPaint(Paint);
            vg.Fill();
        }
    }

    public struct BoxDrawParams
    {
        public RectangleF Rect;
        public ShadowDrawParams? Shadow;
        public FillStyle Fill;
        public float CornerRadius;

        public void Draw(NVGcontext vg)
        {
            Shadow?.Draw(vg, Rect, CornerRadius);

            vg.BeginPath();
            Fill.Apply(vg);
            vg.RoundedRect(Rect.X, Rect.Y, Rect.Width, Rect.Height, CornerRadius);
            vg.Fill();
        }        
    }

    public struct TextDrawParams
    {
        public string Text;
        public float Size;
        public string Font;
        public RectangleF Rect;
        public NVGcolor Color;

        public void Draw(NVGcontext vg)
        {
            vg.FontFace(Font);
            vg.FontSize(Size);
            vg.FillColor(Color);          
            vg.TextBox(Rect.X, Rect.Y, Rect.Width, Text);
            DrawDebugRect(vg);
        }

        public void DrawDebugRect(NVGcontext vg)
        {
            vg.BeginPath();
            vg.Rect(Rect.X, Rect.Y, Rect.Width, Rect.Height);
            vg.StrokeColor("#ff00ff");
            vg.Stroke();
        }
    }

    public class SizeQ
    {
        public static implicit operator SizeF(SizeQ q) => q.ToPointF();

        public Quantity Width;
        public Quantity Height;

        public SizeQ(Quantity width, Quantity height)
        {
            Width = width;
            Height = height;
        }

        public SizeF ToPointF() => new SizeF(Width, Height);
    }

    public class PointQ
    {
        public static implicit operator PointF(PointQ q) => q.ToPointF();
        
        public Quantity X;
        public Quantity Y;

        public PointQ(Quantity x, Quantity y)
        {
            X = x;
            Y = y;
        }

        public PointF ToPointF() => new PointF(X, Y);
    }
    
    public class RectQ
    {
        public static implicit operator RectangleF(RectQ q) => q.ToRectangleF();

        public Quantity X;
        public Quantity Y;
        public Quantity Width;
        public Quantity Height;

        public RectQ(Quantity x, Quantity y, Quantity width, Quantity height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public RectangleF ToRectangleF() => new RectangleF(X, Y, Width, Height);
    } 

    public readonly struct Quantity
    {
        public static implicit operator float(Quantity q) => q.ToPixels();

        public static implicit operator Quantity(float v)
        {
            return new Quantity(v, Unit.Point);
        }

        public static implicit operator Quantity(string str)
        {
            var split = Regex.Split(str, "([0-9\\.]+)([vhwpx%]*)");
            float val = float.Parse(split[1], NumberStyles.Float, CultureInfo.InvariantCulture);
            switch (split[2])
            {
                case "px":
                case "":
                    return new Quantity(val, Unit.Point);
                case "vw":
                    return new Quantity(val, Unit.ViewportWidth);
                case "vh":
                    return new Quantity(val, Unit.ViewportHeight);
                default:
                    throw new Exception("Invalid Unit");
            }
        }

        public readonly float Value;
        public readonly Unit Unit;

        public Quantity(float value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }

        public float ToPixels() 
        {
            var p = IPlatformInfo.Default;
            switch (Unit)
            {
                case Unit.Point:
                    return Value * p.RetinaScale;
                case Unit.ViewportWidth:
                    return (Value / 100f) * p.RendererSize.Width;
                case Unit.ViewportHeight:
                    return (Value / 100f) * p.RendererSize.Height;
                default:
                    throw new Exception("Unknown unit");
            }
}
    }
}
