using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Non-Interactive", Order = 103)]
    public class GroupBoxTest : GUnit
    {
        public GroupBoxTest(ControlBase parent)
            : base(parent)
        {
            GridLayout layout = new GridLayout(this);
            layout.ColumnCount = 2;

            {
                GroupBox gb = new GroupBox(layout);
                gb.Size = new Size(200, 100);
                gb.Text = "Group Box";
            }

            {
                GroupBox gb = new GroupBox(layout);
                gb.HorizontalAlignment = HorizontalAlignment.Left;
                gb.VerticalAlignment = VerticalAlignment.Top;
                gb.Text = "With Label (autosized)";
                Label label = new Label(gb);
                label.Dock = Dock.Fill;
                label.Text = "I'm a label";
            }

            {
                GroupBox gb = new GroupBox(layout);
                gb.HorizontalAlignment = HorizontalAlignment.Left;
                gb.VerticalAlignment = VerticalAlignment.Top;
                gb.Text = "With Label (autosized)";
                Label label = new Label(gb);
                label.Dock = Dock.Fill;
                label.Text = "I'm a label. I'm a really long label!";
            }

            {
                GroupBox gb = new GroupBox(layout);
                gb.HorizontalAlignment = HorizontalAlignment.Left;
                gb.VerticalAlignment = VerticalAlignment.Top;
                gb.Text = "Two docked Labels (autosized)";
                ControlBase gbl = new DockLayout(gb);
                Label label1 = new Label(gbl);
                label1.Text = "I'm a label";
                label1.Dock = Dock.Top;
                Label label2 = new Label(gbl);
                label2.Text = "I'm a label. I'm a really long label!";
                label2.Dock = Dock.Top;
            }

            {
                GroupBox gb = new GroupBox(layout);
                gb.HorizontalAlignment = HorizontalAlignment.Left;
                gb.VerticalAlignment = VerticalAlignment.Top;
                gb.Text = "Empty (autosized)";
            }

            {
                GroupBox gb1 = new GroupBox(layout);
                gb1.HorizontalAlignment = HorizontalAlignment.Left;
                gb1.VerticalAlignment = VerticalAlignment.Top;
                gb1.Padding = Padding.Five;
                gb1.Text = "Yo dawg,";
                ControlBase gb1l = new DockLayout(gb1);

                GroupBox gb2 = new GroupBox(gb1l);
                gb2.Text = "I herd";
                gb2.Dock = Dock.Left;
                gb2.Margin = Margin.Three;
                gb2.Padding = Padding.Five;

                GroupBox gb3 = new GroupBox(gb1l);
                gb3.Text = "You like";
                gb3.Dock = Dock.Fill;
                ControlBase gb3l = new DockLayout(gb3);

                GroupBox gb4 = new GroupBox(gb3l);
                gb4.Text = "Group Boxes,";
                gb4.Dock = Dock.Top;

                GroupBox gb5 = new GroupBox(gb3l);
                gb5.Text = "So I put Group";
                gb5.Dock = Dock.Fill;
                ControlBase gb5l = new DockLayout(gb5);

                GroupBox gb6 = new GroupBox(gb5l);
                gb6.Text = "Boxes in yo";
                gb6.Dock = Dock.Left;

                GroupBox gb7 = new GroupBox(gb5l);
                gb7.Text = "Boxes so you can";
                gb7.Dock = Dock.Top;
                ControlBase gb7l = new DockLayout(gb7);

                GroupBox gb8 = new GroupBox(gb7l);
                gb8.Text = "Group Box while";
                gb8.Dock = Dock.Top;
                gb8.Margin = Margin.Five;

                GroupBox gb9 = new GroupBox(gb7l);
                gb9.Text = "u Group Box";
                gb9.Dock = Dock.Bottom;
                gb9.Padding = Padding.Five;
            }
        }
    }
}