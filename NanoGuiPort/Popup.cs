namespace net6test.NanoGuiPort
{
    internal class Popup : Window
    {
        public Popup(Widget parent, string title) : base(parent, title)
        {
        }

        public Window ParentWindow { get; internal set; }
    }
}