using NanoVGDotNet;
using net6test.UI;

namespace net6test.UI
{
    public class Layout
    {
        public static bool DrawDebugLines { get; set; } = false;

        private readonly NVGcontext vg;

        public ICollection<Element> Children { get; } = new List<Element>();

        public Layout(NVGcontext vg)
        {
            this.vg = vg;
        }

        public void Arrange()
        {
            foreach (var c in Children)
            {
                var ctx = new UiContext 
                { 
                    Vg = vg,
                    MinW = 0,
                    MinH = 0,
                    MaxW = 0,/*IPlatform.Default.RendererSize.Width*/
                    MaxH = 0 /*IPlatform.Default.RendererSize.Height*/                    
                };
                c.Arrange(ctx);
            }
        }

        public void Update()
        {
            foreach (var c in Children)
            {
                c.Update(vg);
            }
        }

        public void Draw()
        {
            //var size = IPlatform.Default.RendererSize;
            //vg.BeginFrame(size.Width, size.Height, 1);
            //foreach (var c in Children)
            //{
            //    c.Draw(vg);
            //}
            //vg.EndFrame();
        }

    }
}