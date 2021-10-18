using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public struct Margin
    {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public Margin(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Margin(float margin) : this(margin,margin,margin,margin)
        {
        }
    }

    public class ShadowPaint
    {
        public NVGpaint Paint { get; set; }
        float Size { get; set; }
        PointF Offset { get; set; }
    }

    public class Style
    {
        public NVGcolor TextColor { get;set; }
        public NVGpaint BackgroundPaint { get;set; }
        public float FontSize { get;set; }
        public ShadowPaint Shadow { get; set; }
        public Margin Margin { get; set; }
        public SizeF Size { get; set; }
        public float BorderRadius { get; set; }
    }

    public class VisualElement
    {
        public VisualElement Parent { get; set; }
        public List<VisualElement> Children { get; } = new List<VisualElement>();
        public bool NeedsLayout { get; set; }
        public RectangleF Bounds { get; set; }
        public Style Style { get; set; }
        public Style HoverStyle { get; set; }
        public string TextContent { get; set; }
        public VisualState State { get; set; }
    }

    public enum VisualState
    {
        Default,
        Hover
    }
}
