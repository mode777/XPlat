using System.Numerics;

namespace net6test.NanoGuiPort
{
    public class ToolButton : Button
    {
        public ToolButton(Widget? parent, int icon, string caption = "") 
            : base(parent, caption, icon)
        {
            Flags = ButtonFlags.ToggleButton | ButtonFlags.RadioButton;
            FixedSize = new Vector2(25,25);
        }
    }
}