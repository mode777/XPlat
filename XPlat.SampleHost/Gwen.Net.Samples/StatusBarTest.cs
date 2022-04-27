using Gwen.Net.Control;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Non-Interactive", Order = 106)]
    public class StatusBarTest : GUnit
    {
        public StatusBarTest(ControlBase parent)
            : base(parent)
        {
            StatusBar sb = new StatusBar(this);
            Label left = new Label(sb);
            left.Text = "Label added to left";
            sb.AddControl(left, false);

            Label right = new Label(sb);
            right.Text = "Label added to right";
            sb.AddControl(right, true);

            Button bl = new Button(sb);
            bl.Text = "Left button";
            sb.AddControl(bl, false);

            Button br = new Button(sb);
            br.Text = "Right button";
            sb.AddControl(br, true);
        }
    }
}