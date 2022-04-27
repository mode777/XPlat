using System;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Non-Interactive", Order = 104)]
    public class ProgressBarTest : GUnit
    {
        public ProgressBarTest(ControlBase parent) : base(parent)
        {
            HorizontalLayout hlayout = new HorizontalLayout(this);
            hlayout.VerticalAlignment = VerticalAlignment.Top;
            {
                VerticalLayout vlayout = new VerticalLayout(hlayout);
                vlayout.Width = 200;
                {
                    {
                        ProgressBar pb = new ProgressBar(vlayout);
                        pb.Margin = Margin.Five;
                        pb.Value = 0.03f;
                    }

                    {
                        ProgressBar pb = new ProgressBar(vlayout);
                        pb.Margin = Margin.Five;
                        pb.Value = 0.66f;
                        pb.Alignment = Alignment.Right | Alignment.CenterV;
                    }

                    {
                        ProgressBar pb = new ProgressBar(vlayout);
                        pb.Margin = Margin.Five;
                        pb.Value = 0.88f;
                        pb.Alignment = Alignment.Left | Alignment.CenterV;
                    }

                    {
                        ProgressBar pb = new ProgressBar(vlayout);
                        pb.Margin = Margin.Five;
                        pb.AutoLabel = false;
                        pb.Value = 0.20f;
                        pb.Alignment = Alignment.Right | Alignment.CenterV;
                        pb.Text = "40,245 MB";
                    }

                    {
                        ProgressBar pb = new ProgressBar(vlayout);
                        pb.Margin = Margin.Five;
                        pb.AutoLabel = false;
                        pb.Value = 1.00f;
                    }

                    {
                        ProgressBar pb = new ProgressBar(vlayout);
                        pb.Margin = Margin.Five;
                        pb.AutoLabel = false;
                        pb.Value = 0.00f;
                    }

                    {
                        ProgressBar pb = new ProgressBar(vlayout);
                        pb.Margin = Margin.Five;
                        pb.AutoLabel = false;
                        pb.Value = 0.50f;
                    }
                }
            }

            {
                ProgressBar pb = new ProgressBar(hlayout);
                pb.Margin = Margin.Five;
                pb.IsHorizontal = false;
                pb.Value = 0.25f;
                pb.Alignment = Alignment.Top | Alignment.CenterH;
            }

            {
                ProgressBar pb = new ProgressBar(hlayout);
                pb.Margin = Margin.Five;
                pb.IsHorizontal = false;
                pb.Value = 0.40f;
            }

            {
                ProgressBar pb = new ProgressBar(hlayout);
                pb.Margin = Margin.Five;
                pb.IsHorizontal = false;
                pb.Alignment = Alignment.Bottom | Alignment.CenterH;
                pb.Value = 0.65f;
            }
        }
    }
}