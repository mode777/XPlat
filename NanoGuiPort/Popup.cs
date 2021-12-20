using System.Numerics;

namespace net6test.NanoGuiPort
{
    public class Popup : Window
    {
        public Popup(Widget parent, Window window) : base(parent, "")
        {
            ParentWindow = window;
            AnchorOffset = 30;
            AnchorSize = 15;
            Side = PopupSide.Right;
        }

        public Vector2 AnchorPos { get; set; }
        public Window ParentWindow { get; set; }
        public float AnchorOffset { get; set; }
        public float AnchorSize { get; set; }
        public PopupSide Side { get; set; }
    }

    public enum PopupSide
    {
        Left,
        Right
    }
}