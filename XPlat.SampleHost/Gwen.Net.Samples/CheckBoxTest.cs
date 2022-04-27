using System;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Standard", Order = 202)]
    public class CheckBoxTest : GUnit
    {
        public CheckBoxTest(ControlBase parent)
            : base(parent)
        {
            VerticalLayout layout = new VerticalLayout(this);

            CheckBox check;

            check = new CheckBox(layout);
            check.Margin = Net.Margin.Three;
            check.Checked += OnChecked;
            check.UnChecked += OnUnchecked;
            check.CheckChanged += OnCheckChanged;

            LabeledCheckBox labeled;

            labeled = new LabeledCheckBox(layout);
            labeled.Margin = Net.Margin.Three;
            labeled.Text = "Labeled CheckBox";
            labeled.Checked += OnChecked;
            labeled.UnChecked += OnUnchecked;
            labeled.CheckChanged += OnCheckChanged;

            check = new CheckBox(layout);
            check.Margin = Net.Margin.Three;
            check.IsDisabled = true;
        }

        void OnChecked(ControlBase control, EventArgs args)
        {
            UnitPrint("CheckBox: Checked");
        }

        void OnCheckChanged(ControlBase control, EventArgs args)
        {
            UnitPrint("CheckBox: CheckChanged");
        }

        void OnUnchecked(ControlBase control, EventArgs args)
        {
            UnitPrint("CheckBox: UnChecked");
        }
    }
}