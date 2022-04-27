using System;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Internal;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Containers", Order = 304)]
    public class TabControlTest : GUnit
    {
        private readonly TabControl m_DockControl;

        public TabControlTest(ControlBase parent)
            : base(parent)
        {
            {
                m_DockControl = new TabControl(this);
                m_DockControl.Margin = Margin.Zero;
                m_DockControl.Width = 200;
                //m_DockControl.Height = 150;
                m_DockControl.Dock = Dock.Top;

                {
                    TabButton button = m_DockControl.AddPage("Controls");
                    ControlBase page = button.Page;

                    {
                        GroupBox group = new GroupBox(page);
                        group.Text = "Tab position";
                        RadioButtonGroup radio = new RadioButtonGroup(group);

                        radio.AddOption("Top").Select();
                        radio.AddOption("Bottom");
                        radio.AddOption("Left");
                        radio.AddOption("Right");

                        radio.SelectionChanged += OnDockChange;
                    }
                }

                m_DockControl.AddPage("Red");
                m_DockControl.AddPage("Green");
                m_DockControl.AddPage("Blue");
                m_DockControl.AddPage("Blue");
                m_DockControl.AddPage("Blue");
            }

            {
                TabControl dragMe = new TabControl(this);
                dragMe.Margin = Margin.Five;
                dragMe.Width = 200;
                dragMe.Dock = Dock.Top;

                dragMe.AddPage("You");
                dragMe.AddPage("Can");
                dragMe.AddPage("Reorder").SetImage("test16.png");
                dragMe.AddPage("These");
                dragMe.AddPage("Tabs");

                dragMe.AllowReorder = true;
            }
        }

        void OnDockChange(ControlBase control, EventArgs args)
        {
            RadioButtonGroup rc = (RadioButtonGroup)control;

            if (rc.SelectedLabel == "Top") m_DockControl.TabStripPosition = Dock.Top;
            if (rc.SelectedLabel == "Bottom") m_DockControl.TabStripPosition = Dock.Bottom;
            if (rc.SelectedLabel == "Left") m_DockControl.TabStripPosition = Dock.Left;
            if (rc.SelectedLabel == "Right") m_DockControl.TabStripPosition = Dock.Right;
        }
    }
}