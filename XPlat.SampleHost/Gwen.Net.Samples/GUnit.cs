using Gwen.Net.Control;

namespace Gwen.Net.Tests.Components
{
    public class GUnit : ControlBase
    {
        public UnitTestHarnessControls UnitTest;

        public GUnit(ControlBase parent) : base(parent)
        {
            this.IsVirtualControl = true;
        }

        public void UnitPrint(string str)
        {
            if (UnitTest != null)
                UnitTest.PrintText(str);
        }
    }
}