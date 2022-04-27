using System;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Internal;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Standard", Order = 207)]
    public class SliderTest : GUnit
    {
        public SliderTest(ControlBase parent)
            : base(parent)
        {
            HorizontalLayout hlayout = new HorizontalLayout(this);

            VerticalLayout vlayout = new VerticalLayout(hlayout);

            {
                HorizontalSlider slider = new HorizontalSlider(vlayout);
                slider.Margin = Margin.Ten;
                slider.Width = 150;
                slider.SetRange(0, 100);
                slider.Value = 25;
                slider.ValueChanged += SliderMoved;
            }

            {
                HorizontalSlider slider = new HorizontalSlider(vlayout);
                slider.Margin = Margin.Ten;
                slider.Width = 150;
                slider.SetRange(0, 100);
                slider.Value = 20;
                slider.NotchCount = 10;
                slider.SnapToNotches = true;
                slider.ValueChanged += SliderMoved;
            }

            {
                VerticalSlider slider = new VerticalSlider(hlayout);
                slider.Margin = Margin.Ten;
                slider.Height = 200;
                slider.SetRange(0, 100);
                slider.Value = 25;
                slider.ValueChanged += SliderMoved;
            }

            {
                VerticalSlider slider = new VerticalSlider(hlayout);
                slider.Margin = Margin.Ten;
                slider.Height = 200;
                slider.SetRange(0, 100);
                slider.Value = 20;
                slider.NotchCount = 10;
                slider.SnapToNotches = true;
                slider.ValueChanged += SliderMoved;
            }
        }

        void SliderMoved(ControlBase control, EventArgs args)
        {
            Slider slider = control as Slider;
            UnitPrint(String.Format("Slider moved: ValueChanged: {0}", slider.Value));
        }
    }
}