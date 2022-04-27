using System;
using Gwen.Net.Control;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Non-standard", Order = 500)]
    public class CollapsibleListTest : GUnit
    {
        public CollapsibleListTest(ControlBase parent)
            : base(parent)
        {
            CollapsibleList control = new CollapsibleList(this);
            control.Dock = Net.Dock.Fill;
            control.HorizontalAlignment = Net.HorizontalAlignment.Left;
            control.ItemSelected += OnSelection;
            control.CategoryCollapsed += OnCollapsed;

            {
                CollapsibleCategory cat = control.Add("Category One");
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }

            {
                CollapsibleCategory cat = control.Add("Shopping");
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
            }

            {
                CollapsibleCategory cat = control.Add("Category One");
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }
        }

        void OnSelection(ControlBase control, EventArgs args)
        {
            CollapsibleList list = control as CollapsibleList;
            UnitPrint(String.Format("CollapsibleList: Selected: {0}", list.GetSelectedButton().Text));
        }

        void OnCollapsed(ControlBase control, EventArgs args)
        {
            CollapsibleCategory cat = control as CollapsibleCategory;
            UnitPrint(String.Format("CollapsibleCategory: CategoryCollapsed: {0} {1}", cat.Text, cat.IsCollapsed));
        }
    }
}