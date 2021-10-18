using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NanoVGDotNet;
using System.Globalization;
using System.Text.RegularExpressions;

namespace net6test.UI
{
    public readonly struct Quantity {
        public static implicit operator Quantity(string str){
            var split = Regex.Split(str, "([0-9\\.]+)([vhwpx%]*)");
            float val = float.Parse(split[1], NumberStyles.Float, CultureInfo.InvariantCulture);
            switch(split[2]){
                case "px":
                case "":
                    return new Quantity(val, Unit.Pixel);
                case "%":
                    return new Quantity(val, Unit.Percent);
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
    }

    public enum Unit {
        Pixel,
        Percent,
        ViewportHeight,
        ViewportWidth
    }

    public struct Thickness
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public Thickness(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Thickness(float margin) : this(margin,margin,margin,margin)
        {
        }
    }

    public class ShadowStyle
    {
        public NVGpaint Paint { get; set; }
        Quantity Size { get; set; }
        PointF Offset { get; set; }
    }

    public abstract class FillStyle {
        public static implicit operator FillStyle(string str){
            if(str[0] == '#'){
                return new ColorFillStyle(str);
            } else {
                throw new NotImplementedException();
            }
        }
        public abstract void Apply(NVGcontext vg);
    }

    public class ColorFillStyle : FillStyle {
        public NVGcolor Color;

        public ColorFillStyle(NVGcolor color)
        {
            Color = color;
        }

        public override void Apply(NVGcontext vg)
        {
            vg.FillColor(Color);
        }
    }

    public class GradientFileStyle : FillStyle {
        public NVGpaint Paint;

        public GradientFileStyle(NVGpaint paint)
        {
            Paint = paint;
        }

        public override void Apply(NVGcontext vg)
        {
            vg.FillPaint(Paint);
        }
    }

    public class Style
    {
        public NVGcolor TextColor { get;set; }
        public FillStyle Background { get;set; }
        public float FontSize { get;set; }
        public ShadowStyle Shadow { get; set; }
        public Thickness Margin { get; set; }
        public Thickness Padding { get; set; }
        public Quantity Width { get; set; } = "100%";
        public Quantity Height { get; set; } = "100%";
        public float BorderRadius { get; set; }
        public int ZIndex { get; set; } = 0;
    }

    public class VisualElement
    {
        public VisualElement Parent { get; set; }
        public List<VisualElement> Children { get; set; } = new List<VisualElement>();
        public bool NeedsLayout { get; set; }
        public RectangleF Bounds { get; set; }
        public Style Style { get; set; }
        public Style HoverStyle { get; set; }
        public string TextContent { get; set; }
        public VisualState State { get; set; }
        public object Tag { get; set; }
        public string Id { get; set; }
        public FlowDirection Direction { get; set; } = FlowDirection.Vertical;

        public void Init(){
            foreach (var c in Children)
            {
                c.Parent = this;
                c.Init();
            }
        }
    }

    public enum VisualState
    {
        Default,
        Hover
    }

    public enum FlowDirection
    {
        Horizontal,
        Vertical
    }

    public static class VisualTree 
    {


    }
}
