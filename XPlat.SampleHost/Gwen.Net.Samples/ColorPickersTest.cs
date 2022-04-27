using System;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Non-standard", Order = 501)]
    public class ColorPickersTest : GUnit
    {
        public ColorPickersTest(ControlBase parent)
            : base(parent)
        {
            /* RGB Picker */
            {
                ColorPicker rgbPicker = new ColorPicker(this);
                rgbPicker.Dock = Net.Dock.Top;
                rgbPicker.ColorChanged += ColorChanged;
            }

            /* HSVColorPicker */
            {
                HSVColorPicker hsvPicker = new HSVColorPicker(this);
                hsvPicker.Dock = Net.Dock.Fill;
                hsvPicker.HorizontalAlignment = Net.HorizontalAlignment.Left;
                hsvPicker.VerticalAlignment = Net.VerticalAlignment.Top;
                hsvPicker.ColorChanged += ColorChanged;
            }

            /* HSVColorPicker in Window */
            {
                WindowTest window = new WindowTest(base.GetCanvas());
                window.Size = new Net.Size(300, 200);
                window.Collapse();
                DockLayout layout = new DockLayout(window);

                HSVColorPicker hsvPicker = new HSVColorPicker(layout);
                hsvPicker.Margin = Net.Margin.Two;
                hsvPicker.Dock = Net.Dock.Fill;
                hsvPicker.ColorChanged += ColorChanged;

                Button OpenWindow = new Button(this);
                OpenWindow.Dock = Net.Dock.Bottom;
                OpenWindow.HorizontalAlignment = Net.HorizontalAlignment.Left;
                OpenWindow.Text = "Open Window";
                OpenWindow.Clicked += delegate (ControlBase sender, ClickedEventArgs args)
                {
                    window.Show();
                };
            }
        }

        void ColorChanged(ControlBase control, EventArgs args)
        {
            IColorPicker picker = control as IColorPicker;
            Net.Color c = picker.SelectedColor;
            Net.HSV hsv = c.ToHSV();
            String text = String.Format("Color changed: RGB: {0:X2}{1:X2}{2:X2} HSV: {3:F1} {4:F2} {5:F2}",
                                        c.R, c.G, c.B, hsv.H, hsv.S, hsv.V);
            UnitPrint(text);
        }
    }
}