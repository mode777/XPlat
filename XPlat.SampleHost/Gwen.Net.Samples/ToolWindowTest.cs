using System;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Containers", Order = 301)]
    public class ToolWindowTest : GUnit
    {
        public ToolWindowTest(ControlBase parent)
            : base(parent)
        {
            VerticalLayout layout = new VerticalLayout(this);
            layout.HorizontalAlignment = HorizontalAlignment.Left;

            Button button;

            button = new Button(layout);
            button.Margin = Margin.Five;
            button.Text = "Open a ToolBar";
            button.Clicked += OpenToolBar;

            button = new Button(layout);
            button.Margin = Margin.Five;
            button.Text = "Open a tool window";
            button.Clicked += OpenToolWindow;
        }

        void OpenToolBar(ControlBase control, EventArgs args)
        {
            ToolWindow window = new ToolWindow(this);
            window.Padding = Padding.Five;
            window.HorizontalAlignment = HorizontalAlignment.Left;
            window.VerticalAlignment = VerticalAlignment.Top;
            window.StartPosition = StartPosition.CenterCanvas;

            HorizontalLayout layout = new HorizontalLayout(window);

            for (int i = 0; i < 5; i++)
            {
                Button button = new Button(layout);
                button.Size = new Size(36, 36);
                button.UserData = window;
                button.Clicked += Close;
            }
        }

        void OpenToolWindow(ControlBase control, EventArgs args)
        {
            ToolWindow window = new ToolWindow(this);
            window.Padding = Padding.Five;
            window.HorizontalAlignment = HorizontalAlignment.Left;
            window.VerticalAlignment = VerticalAlignment.Top;
            window.StartPosition = StartPosition.CenterParent;
            window.Vertical = true;

            GridLayout layout = new GridLayout(window);
            layout.ColumnCount = 2;

            Button button = new Button(layout);
            button.Size = new Size(100, 40);
            button.UserData = window;
            button.Clicked += Close;

            button = new Button(layout);
            button.Size = new Size(100, 40);
            button.UserData = window;
            button.Clicked += Close;

            button = new Button(layout);
            button.Size = new Size(100, 40);
            button.UserData = window;
            button.Clicked += Close;

            button = new Button(layout);
            button.Size = new Size(100, 40);
            button.UserData = window;
            button.Clicked += Close;
        }

        void Close(ControlBase control, EventArgs args)
        {
            ToolWindow window = control.UserData as ToolWindow;
            window.Close();
            window.Parent.RemoveChild(window, true);
        }
    }
}