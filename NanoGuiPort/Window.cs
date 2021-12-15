namespace net6test.NanoGuiPort
{
    public class Window : Widget
    {
        public Window(Widget parent, string title) : base(parent)
        {
        }

        public bool Modal { get; internal set; }
    }
}