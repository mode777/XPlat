using System.Drawing;

namespace net6test.UI
{
    public class LayoutContext 
    {
        public SizeF Viewport { get; set; }
        
        public SizeF MaxSize { get; set; }
        public SizeF MinSize { get; set; }

    }

    public interface ILayout
    {   
        SizeF GetMarginSize(VisualElement element, LayoutContext context);
        SizeF ArrangeMarginBox(VisualElement element, LayoutContext context);
    }

    public class DockLayout : ILayout
    {
        public SizeF ArrangeMarginBox(VisualElement element, LayoutContext context)
        {
            throw new NotImplementedException();
        }

        public SizeF GetMarginSize(VisualElement element, LayoutContext context)
        {
            

        }
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
        public ILayout Layout { get; set; }


        public void Init(){
            foreach (var c in Children)
            {
                c.Parent = this;
                c.Init();
            }
        }
    }
}
