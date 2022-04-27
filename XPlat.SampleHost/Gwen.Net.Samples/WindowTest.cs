using System;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Containers", Order = 300)]
    public class WindowTest : GUnit
    {
        private int m_WindowCount;
        private readonly Random m_Rand;

        public WindowTest(ControlBase parent)
            : base(parent)
        {
            m_Rand = new Random();

            VerticalLayout layout = new VerticalLayout(this);
            layout.HorizontalAlignment = HorizontalAlignment.Left;

            Button button;

            button = new Button(layout);
            button.Margin = Margin.Five;
            button.Text = "Open a Window";
            button.Clicked += OpenWindow;

            button = new Button(layout);
            button.Margin = Margin.Five;
            button.Text = "Open a Window (with menu)";
            button.Clicked += OpenWindowWithMenuAndStatusBar;

            button = new Button(layout);
            button.Margin = Margin.Five;
            button.Text = "Open a Window (auto size)";
            button.Clicked += OpenWindowAutoSizing;

            button = new Button(layout);
            button.Margin = Margin.Five;
            button.Text = "Open a Window (modal)";
            button.Clicked += OpenWindowModal;

            button = new Button(layout);
            button.Margin = Margin.Five;
            button.Text = "Open a MessageBox";
            button.Clicked += OpenMsgbox;

            button = new Button(layout);
            button.Margin = Margin.Five;
            button.Text = "Open a Long MessageBox";
            button.Clicked += OpenLongMsgbox;

            m_WindowCount = 0;
        }

        private void OpenWindow(ControlBase control, EventArgs args)
        {
            Window window = new Window(this);
            window.Title = String.Format("Window ({0})", ++m_WindowCount);
            window.Size = new Size(m_Rand.Next(200, 400), m_Rand.Next(200, 400));
            window.Left = m_Rand.Next(700);
            window.Top = m_Rand.Next(400);
            window.Padding = new Padding(6, 3, 6, 6);

            RadioButtonGroup rbg = new RadioButtonGroup(window);
            rbg.Dock = Dock.Top;
            rbg.AddOption("Resize disabled", "None").Checked += (c, a) => window.Resizing = Resizing.None;
            rbg.AddOption("Resize width", "Width").Checked += (c, a) => window.Resizing = Resizing.Width;
            rbg.AddOption("Resize height", "Height").Checked += (c, a) => window.Resizing = Resizing.Height;
            rbg.AddOption("Resize both", "Both").Checked += (c, a) => window.Resizing = Resizing.Both;
            rbg.SetSelectionByName("Both");

            LabeledCheckBox dragging = new LabeledCheckBox(window);
            dragging.Dock = Dock.Top;
            dragging.Text = "Dragging";
            dragging.IsChecked = true;
            dragging.CheckChanged += (c, a) => window.IsDraggingEnabled = dragging.IsChecked;
        }

        private void OpenWindowWithMenuAndStatusBar(ControlBase control, EventArgs args)
        {
            Window window = new Window(this);
            window.Title = String.Format("Window ({0})", ++m_WindowCount);
            window.Size = new Size(m_Rand.Next(200, 400), m_Rand.Next(200, 400));
            window.Left = m_Rand.Next(700);
            window.Top = m_Rand.Next(400);
            window.Padding = new Padding(1, 0, 1, 1);

            DockLayout layout = new DockLayout(window);

            MenuStrip menuStrip = new MenuStrip(layout);
            menuStrip.Dock = Dock.Top;

            /* File */
            {
                MenuItem root = menuStrip.AddItem("File");
                root.Menu.AddItem("Load", "test16.png", "Ctrl+L");
                root.Menu.AddItem("Save", String.Empty, "Ctrl+S");
                root.Menu.AddItem("Save As..", String.Empty, "Ctrl+A");
                root.Menu.AddItem("Quit", String.Empty, "Ctrl+Q").SetAction((c, a) => window.Close());
            }
            /* Resizing */
            {
                MenuItem root = menuStrip.AddItem("Resizing");
                root.Menu.AddItem("Disabled").SetAction((c, a) => window.Resizing = Resizing.None);
                root.Menu.AddItem("Width").SetAction((c, a) => window.Resizing = Resizing.Width);
                root.Menu.AddItem("Height").SetAction((c, a) => window.Resizing = Resizing.Height);
                root.Menu.AddItem("Both").SetAction((c, a) => window.Resizing = Resizing.Both);
            }

            StatusBar statusBar = new StatusBar(layout);
            statusBar.Dock = Dock.Bottom;
            statusBar.Text = "Status bar";

            {
                Button br = new Button(statusBar);
                br.Text = "Right button";
                statusBar.AddControl(br, true);
            }
        }

        private void OpenWindowAutoSizing(ControlBase control, EventArgs args)
        {
            Window window = new Window(this);
            window.Title = String.Format("Window ({0})", ++m_WindowCount);
            window.Left = m_Rand.Next(700);
            window.Top = m_Rand.Next(400);
            window.Padding = new Padding(6, 3, 6, 6);
            window.HorizontalAlignment = HorizontalAlignment.Left;
            window.VerticalAlignment = VerticalAlignment.Top;
            window.Resizing = Resizing.None;

            VerticalLayout layout = new VerticalLayout(window);

            GroupBox grb = new GroupBox(layout);
            grb.Text = "Auto size";
            layout = new VerticalLayout(grb);

            {
                Label label = new Label(layout);
                label.Margin = Margin.Six;
                label.Text = "Label text";

                Button button = new Button(layout);
                button.Margin = Margin.Six;
                button.Text = "Click Me";
                button.Width = 200;

                label = new Label(layout);
                label.Margin = Margin.Six;
                label.Text = "Hide / Show Label";
                //label.IsCollapsed = true;

                button.Clicked += (s, a) => label.IsCollapsed = !label.IsCollapsed;
            }
        }

        private void OpenWindowModal(ControlBase control, EventArgs args)
        {
            Window window = new Window(this);
            window.Title = String.Format("Modal Window ({0})", ++m_WindowCount);
            window.Left = m_Rand.Next(700);
            window.Top = m_Rand.Next(400);
            window.Padding = new Padding(6, 3, 6, 6);
            window.HorizontalAlignment = HorizontalAlignment.Left;
            window.VerticalAlignment = VerticalAlignment.Top;
            window.Resizing = Resizing.None;
            window.MakeModal(true);

            VerticalLayout layout = new VerticalLayout(window);

            GroupBox grb = new GroupBox(layout);
            grb.Text = "Auto size";
            layout = new VerticalLayout(grb);

            {
                Label label = new Label(layout);
                label.Margin = Margin.Six;
                label.Text = "Label text";

                Button button = new Button(layout);
                button.Margin = Margin.Six;
                button.Text = "Button";
                button.Width = 200;
            }
        }

        private void OpenMsgbox(ControlBase control, EventArgs args)
        {
            MessageBox window = new MessageBox(this, "Message box test text.");
            window.Dismissed += OnDismissed;
            window.SetPosition(m_Rand.Next(700), m_Rand.Next(400));
        }

        private void OpenLongMsgbox(ControlBase control, EventArgs args)
        {
            MessageBox window = new MessageBox(this, @"In olden times when wishing still helped one, there lived a king whose daughters were all beautiful, but the youngest was so beautiful that the sun itself, which has seen so much, was astonished whenever it shone in her face. Close by the king's castle lay a great dark forest, and under an old lime-tree in the forest was a well, and when the day was very warm, the king's child went out into the forest and sat down by the side of the cool fountain, and when she was bored she took a golden ball, and threw it up on high and caught it, and this ball was her favorite plaything.", "Long Text", MessageBoxButtons.AbortRetryIgnore);
            window.Dismissed += OnDismissed;
            window.SetPosition(m_Rand.Next(700), m_Rand.Next(400));
        }

        private void OnDismissed(ControlBase sender, MessageBoxResultEventArgs args)
        {
            UnitTest.PrintText("Message box result: " + args.Result);
        }
    }
}