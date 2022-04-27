using System;
using Gwen.Net.Control;

namespace Gwen.Net.DragDrop
{
    public class Package
    {
        public string Name;
        public object UserData;
        public bool IsDraggable;
        public ControlBase DrawControl;
        public Point HoldOffset;
    }
}