using System;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Layout", Order = 400)]
    public class DockingTest : GUnit
    {
        private readonly Font font;
        private ControlBase outer;

        public DockingTest(ControlBase parent)
            : base(parent)
        {
            font = Skin.DefaultFont.Copy();
            font.Size *= 2;

            Label inner1, inner2, inner3, inner4, inner5;

            HorizontalLayout hlayout = new HorizontalLayout(this);
            {
                VerticalLayout vlayout = new VerticalLayout(hlayout);
                {
                    outer = new DockLayout(vlayout);
                    outer.Size = new Size(400, 400);
                    {
                        inner1 = new Label(outer);
                        inner1.Alignment = Alignment.Center;
                        inner1.Text = "1";
                        inner1.Font = font;
                        inner1.Size = new Size(100, Util.Ignore);
                        inner1.Dock = Dock.Left;

                        inner2 = new Label(outer);
                        inner2.Alignment = Alignment.Center;
                        inner2.Text = "2";
                        inner2.Font = font;
                        inner2.Size = new Size(Util.Ignore, 100);
                        inner2.Dock = Dock.Top;

                        inner3 = new Label(outer);
                        inner3.Alignment = Alignment.Center;
                        inner3.Text = "3";
                        inner3.Font = font;
                        inner3.Size = new Size(100, Util.Ignore);
                        inner3.Dock = Dock.Right;

                        inner4 = new Label(outer);
                        inner4.Alignment = Alignment.Center;
                        inner4.Text = "4";
                        inner4.Font = font;
                        inner4.Size = new Size(Util.Ignore, 100);
                        inner4.Dock = Dock.Bottom;

                        inner5 = new Label(outer);
                        inner5.Alignment = Alignment.Center;
                        inner5.Text = "5";
                        inner5.Font = font;
                        inner5.Size = new Size(Util.Ignore, Util.Ignore);
                        inner5.Dock = Dock.Fill;
                    }

                    outer.DrawDebugOutlines = true;

                    HorizontalLayout hlayout2 = new HorizontalLayout(vlayout);
                    {
                        Label l_padding = new Label(hlayout2);
                        l_padding.Text = "Padding:";

                        HorizontalSlider padding = new HorizontalSlider(hlayout2);
                        padding.Min = 0;
                        padding.Max = 200;
                        padding.Value = 10;
                        padding.Width = 100;
                        padding.ValueChanged += PaddingChanged;
                    }
                }

                GridLayout controlsLayout = new GridLayout(hlayout);
                controlsLayout.ColumnCount = 2;
                {
                    inner1.UserData = CreateControls(inner1, Dock.Left, "Control 1", controlsLayout);
                    inner2.UserData = CreateControls(inner2, Dock.Top, "Control 2", controlsLayout);
                    inner3.UserData = CreateControls(inner3, Dock.Right, "Control 3", controlsLayout);
                    inner4.UserData = CreateControls(inner4, Dock.Bottom, "Control 4", controlsLayout);
                    inner5.UserData = CreateControls(inner5, Dock.Fill, "Control 5", controlsLayout);
                }
            }
            //DrawDebugOutlines = true;
        }

        ControlBase CreateControls(ControlBase subject, Dock docking, string name, ControlBase container)
        {
            GroupBox gb = new GroupBox(container);
            gb.Text = name;
            {
                HorizontalLayout hlayout = new HorizontalLayout(gb);
                {
                    GroupBox dgb = new GroupBox(hlayout);
                    dgb.Text = "Dock";
                    {
                        RadioButtonGroup dock = new RadioButtonGroup(dgb);
                        dock.UserData = subject;
                        dock.AddOption("Left", null, Dock.Left);
                        dock.AddOption("Top", null, Dock.Top);
                        dock.AddOption("Right", null, Dock.Right);
                        dock.AddOption("Bottom", null, Dock.Bottom);
                        dock.AddOption("Fill", null, Dock.Fill);
                        dock.SelectByUserData(docking);
                        dock.SelectionChanged += DockChanged;
                    }

                    VerticalLayout vlayout = new VerticalLayout(hlayout);
                    {
                        HorizontalLayout hlayout2 = new HorizontalLayout(vlayout);
                        {
                            GroupBox hgb = new GroupBox(hlayout2);
                            hgb.Text = "H. Align";
                            {
                                RadioButtonGroup halign = new RadioButtonGroup(hgb);
                                halign.UserData = subject;
                                halign.AddOption("Left", null, HorizontalAlignment.Left);
                                halign.AddOption("Center", null, HorizontalAlignment.Center);
                                halign.AddOption("Right", null, HorizontalAlignment.Right);
                                halign.AddOption("Stretch", null, HorizontalAlignment.Stretch);
                                halign.SelectByUserData(subject.HorizontalAlignment);
                                halign.SelectionChanged += HAlignChanged;
                            }

                            GroupBox vgb = new GroupBox(hlayout2);
                            vgb.Text = "V. Align";
                            {
                                RadioButtonGroup valign = new RadioButtonGroup(vgb);
                                valign.UserData = subject;
                                valign.AddOption("Top", null, VerticalAlignment.Top);
                                valign.AddOption("Center", null, VerticalAlignment.Center);
                                valign.AddOption("Bottom", null, VerticalAlignment.Bottom);
                                valign.AddOption("Stretch", null, VerticalAlignment.Stretch);
                                valign.SelectByUserData(subject.VerticalAlignment);
                                valign.SelectionChanged += VAlignChanged;
                            }
                        }

                        GridLayout glayout = new GridLayout(vlayout);
                        glayout.SetColumnWidths(GridLayout.AutoSize, GridLayout.Fill);
                        {
                            Label l_width = new Label(glayout);
                            l_width.Text = "Width:";

                            HorizontalSlider width = new HorizontalSlider(glayout);
                            width.Name = "Width";
                            width.UserData = subject;
                            width.Min = 50;
                            width.Max = 350;
                            width.Value = 100;
                            width.ValueChanged += WidthChanged;

                            Label l_height = new Label(glayout);
                            l_height.Text = "Height:";

                            HorizontalSlider height = new HorizontalSlider(glayout);
                            height.Name = "Height";
                            height.UserData = subject;
                            height.Min = 50;
                            height.Max = 350;
                            height.Value = 100;
                            height.ValueChanged += HeightChanged;

                            Label l_margin = new Label(glayout);
                            l_margin.Text = "Margin:";

                            HorizontalSlider margin = new HorizontalSlider(glayout);
                            margin.Name = "Margin";
                            margin.UserData = subject;
                            margin.Min = 0;
                            margin.Max = 50;
                            margin.Value = 0;
                            margin.ValueChanged += MarginChanged;
                        }
                    }
                }
            }

            return gb;
        }

        void PaddingChanged(ControlBase control, EventArgs args)
        {
            Net.Control.Internal.Slider val = control as Net.Control.Internal.Slider;
            int i = (int)val.Value;
            outer.Padding = new Padding(i, i, i, i);
        }

        void MarginChanged(ControlBase control, EventArgs args)
        {
            ControlBase inner = control.UserData as ControlBase;
            Net.Control.Internal.Slider val = control as Net.Control.Internal.Slider;
            int i = (int)val.Value;
            inner.Margin = new Margin(i, i, i, i);
        }

        void WidthChanged(ControlBase control, EventArgs args)
        {
            ControlBase inner = control.UserData as ControlBase;
            Net.Control.Internal.Slider val = control as Net.Control.Internal.Slider;
            if (inner.HorizontalAlignment != HorizontalAlignment.Stretch)
                inner.Width = (int)val.Value;
        }

        void HeightChanged(ControlBase control, EventArgs args)
        {
            ControlBase inner = control.UserData as ControlBase;
            Net.Control.Internal.Slider val = control as Net.Control.Internal.Slider;
            if (inner.VerticalAlignment != VerticalAlignment.Stretch)
                inner.Height = (int)val.Value;
        }

        void HAlignChanged(ControlBase control, EventArgs args)
        {
            ControlBase inner = control.UserData as ControlBase;
            RadioButtonGroup rbg = (RadioButtonGroup)control;
            inner.HorizontalAlignment = (HorizontalAlignment)rbg.Selected.UserData;
            if (inner.HorizontalAlignment == HorizontalAlignment.Stretch)
                inner.Width = Util.Ignore;
        }

        void VAlignChanged(ControlBase control, EventArgs args)
        {
            ControlBase inner = control.UserData as ControlBase;
            RadioButtonGroup rbg = (RadioButtonGroup)control;
            inner.VerticalAlignment = (VerticalAlignment)rbg.Selected.UserData;
            if (inner.VerticalAlignment == VerticalAlignment.Stretch)
                inner.Height = Util.Ignore;
        }

        void DockChanged(ControlBase control, EventArgs args)
        {
            ControlBase inner = (ControlBase)control.UserData;
            RadioButtonGroup rbg = (RadioButtonGroup)control;
            ControlBase gb = inner.UserData as ControlBase;
            int w = (int)(gb.FindChildByName("Width", true) as Net.Control.Internal.Slider).Value;
            int h = (int)(gb.FindChildByName("Height", true) as Net.Control.Internal.Slider).Value;
            inner.Dock = (Dock)rbg.Selected.UserData;

            switch (inner.Dock)
            {
                case Dock.Left:
                    inner.Size = new Size(w, Util.Ignore);
                    break;
                case Dock.Top:
                    inner.Size = new Size(Util.Ignore, h);
                    break;
                case Dock.Right:
                    inner.Size = new Size(w, Util.Ignore);
                    break;
                case Dock.Bottom:
                    inner.Size = new Size(Util.Ignore, h);
                    break;
                case Dock.Fill:
                    inner.Size = new Size(Util.Ignore, Util.Ignore);
                    break;
            }
        }

        public override void Dispose()
        {
            font.Dispose();
            base.Dispose();
        }
    }
}