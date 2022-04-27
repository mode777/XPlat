using System;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Standard", Order = 200)]
    public class ButtonTest : GUnit
    {
        public ButtonTest(ControlBase parent)
            : base(parent)
        {
            HorizontalLayout hlayout = new HorizontalLayout(this);
            {
                VerticalLayout vlayout = new VerticalLayout(hlayout);
                vlayout.Width = 300;
                {
                    Button button;

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Text = "Button";

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "Image button (default)";
                    button.SetImage("test16.png");

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "Image button (above)";
                    button.SetImage("test16.png", ImageAlign.Above);

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Alignment = Net.Alignment.Left | Net.Alignment.CenterV;
                    button.Text = "Image button (left)";
                    button.SetImage("test16.png");

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Alignment = Net.Alignment.Right | Net.Alignment.CenterV;
                    button.Text = "Image button (right)";
                    button.SetImage("test16.png");

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "Image button (image left)";
                    button.SetImage("test16.png", ImageAlign.Left | ImageAlign.CenterV);

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "Image button (image right)";
                    button.SetImage("test16.png", ImageAlign.Right | ImageAlign.CenterV);

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "Image button (image fill)";
                    button.SetImage("test16.png", ImageAlign.Fill);

                    HorizontalLayout hlayout2 = new HorizontalLayout(vlayout);
                    {
                        button = new Button(hlayout2);
                        button.HorizontalAlignment = Net.HorizontalAlignment.Left;
                        button.Padding = Net.Padding.Three;
                        button.Margin = Net.Margin.Five;
                        button.SetImage("test16.png");
                        button.ImageSize = new Net.Size(32, 32);

                        button = new Button(hlayout2);
                        button.HorizontalAlignment = Net.HorizontalAlignment.Left;
                        button.VerticalAlignment = Net.VerticalAlignment.Center;
                        button.Padding = Net.Padding.Three;
                        button.Margin = Net.Margin.Five;
                        button.SetImage("test16.png");

                        button = new Button(hlayout2);
                        button.HorizontalAlignment = Net.HorizontalAlignment.Left;
                        button.VerticalAlignment = Net.VerticalAlignment.Center;
                        button.Padding = Net.Padding.Three;
                        button.Margin = Net.Margin.Five;
                        button.SetImage("test16.png");
                        button.ImageTextureRect = new Net.Rectangle(4, 4, 8, 8);

                        button = new Button(hlayout2);
                        button.HorizontalAlignment = Net.HorizontalAlignment.Left;
                        button.VerticalAlignment = Net.VerticalAlignment.Center;
                        button.Padding = Net.Padding.Three;
                        button.Margin = Net.Margin.Five;
                        button.SetImage("test16.png");
                        button.ImageColor = Net.Color.DarkGrey;
                    }

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = new Net.Padding(20, 20, 20, 20);
                    button.Text = "Toggle me";
                    button.IsToggle = true;
                    button.Toggled += onToggle;
                    button.ToggledOn += onToggleOn;
                    button.ToggledOff += onToggleOff;

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "Disabled";
                    button.IsDisabled = true;

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "With Tooltip";
                    button.SetToolTipText("This is tooltip");

                    button = new Button(vlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "Autosized";
                    button.HorizontalAlignment = Net.HorizontalAlignment.Left;
                }

                {
                    Button button = new Button(hlayout);
                    button.Margin = Net.Margin.Five;
                    button.Padding = Net.Padding.Three;
                    button.Text = "Event tester";
                    button.Size = new Net.Size(300, 200);
                    button.Pressed += onButtonAp;
                    button.Clicked += onButtonAc;
                    button.Released += onButtonAr;
                }
            }
        }

        private void onButtonAc(ControlBase control, EventArgs args)
        {
            UnitPrint("Button: Clicked");
        }

        private void onButtonAp(ControlBase control, EventArgs args)
        {
            UnitPrint("Button: Pressed");
        }

        private void onButtonAr(ControlBase control, EventArgs args)
        {
            UnitPrint("Button: Released");
        }

        private void onToggle(ControlBase control, EventArgs args)
        {
            UnitPrint("Button: Toggled");
        }

        private void onToggleOn(ControlBase control, EventArgs args)
        {
            UnitPrint("Button: ToggleOn");
        }

        private void onToggleOff(ControlBase control, EventArgs args)
        {
            UnitPrint("Button: ToggledOff");
        }
    }
}