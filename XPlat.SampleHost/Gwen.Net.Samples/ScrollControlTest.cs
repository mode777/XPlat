using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Containers", Order = 305)]
    public class ScrollControlTest : GUnit
    {
        public ScrollControlTest(ControlBase parent)
            : base(parent)
        {
            GridLayout layout = new GridLayout(this);
            layout.ColumnCount = 6;

            Button pTestButton;

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Twice As Big";
                pTestButton.Size = new Size(200, 200);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Same Size";
                pTestButton.Size = new Size(100, 100);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Wide";
                pTestButton.Size = new Size(200, 50);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Tall";
                pTestButton.Size = new Size(50, 200);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);
                ctrl.EnableScroll(false, true);

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Vertical";
                pTestButton.Size = new Size(200, 200);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);
                ctrl.EnableScroll(true, false);

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Horizontal";
                pTestButton.Size = new Size(200, 200);
            }

            // Bottom Row

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);
                ctrl.AutoHideBars = true;

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Twice As Big";
                pTestButton.Size = new Size(200, 200);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);
                ctrl.AutoHideBars = true;

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Same Size";
                pTestButton.Size = new Size(100, 100);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);
                ctrl.AutoHideBars = true;

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Wide";
                pTestButton.Size = new Size(200, 50);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);
                ctrl.AutoHideBars = true;

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Tall";
                pTestButton.Size = new Size(50, 200);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);
                ctrl.AutoHideBars = true;
                ctrl.EnableScroll(false, true);

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Vertical";
                pTestButton.Size = new Size(200, 200);
            }

            {
                ScrollControl ctrl = new ScrollControl(layout);
                ctrl.Margin = Margin.Three;
                ctrl.Size = new Size(100, 100);
                ctrl.AutoHideBars = true;
                ctrl.EnableScroll(true, false);

                pTestButton = new Button(ctrl);
                pTestButton.Text = "Horinzontal";
                pTestButton.Size = new Size(200, 200);
            }
        }
    }
}