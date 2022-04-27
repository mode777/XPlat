using System;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Layout", Order = 401)]
    public class FlowLayoutTest : GUnit
    {
        public FlowLayoutTest(ControlBase parent)
            : base(parent)
        {
            ControlBase layout = new DockLayout(this);

            FlowLayout flowLayout = new FlowLayout(layout);
            flowLayout.Width = 200;
            flowLayout.Padding = Net.Padding.Five;
            flowLayout.Dock = Net.Dock.Fill;
            flowLayout.DrawDebugOutlines = true;
            {
                Button button;
                int buttonNum = 1;
                const int buttonCount = 10;

                for (int n = 0; n < buttonCount; n++)
                {
                    button = new Button(flowLayout);
                    button.VerticalAlignment = Net.VerticalAlignment.Top;
                    button.HorizontalAlignment = Net.HorizontalAlignment.Left;
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Five;
                    button.ShouldDrawBackground = false;
                    button.Text = String.Format("Button {0}", buttonNum++);
                    button.SetImage("test16.png", ImageAlign.Above);
                }
            }

            HorizontalSlider flowLayoutWidth = new HorizontalSlider(layout);
            flowLayoutWidth.Margin = Net.Margin.Five;
            flowLayoutWidth.Width = 500;
            flowLayoutWidth.Dock = Net.Dock.Top;
            flowLayoutWidth.Min = 50;
            flowLayoutWidth.Max = 500;
            flowLayoutWidth.Value = flowLayout.Width;
            flowLayoutWidth.ValueChanged += (control, args) => { flowLayout.Width = (int)flowLayoutWidth.Value; };
        }
    }
}