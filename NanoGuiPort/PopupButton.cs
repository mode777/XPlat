namespace net6test.NanoGuiPort
{
    internal class PopupButton : Button
    {
        public PopupButton(Widget? parent, string caption, int icon) : base(parent, caption, icon)
        {
        }

        public Widget Popup { get; set; }
    }
}