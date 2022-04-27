using System;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Standard", Order = 204)]
    public class ComboBoxTest : GUnit
    {
        public ComboBoxTest(ControlBase parent)
            : base(parent)
        {
            VerticalLayout layout = new VerticalLayout(this);

            {
                ComboBox combo = new ComboBox(layout);
                combo.Margin = Net.Margin.Five;
                combo.Width = 200;

                combo.AddItem("Option One", "one");
                combo.AddItem("Number Two", "two");
                combo.AddItem("Door Three", "three");
                combo.AddItem("Four Legs", "four");
                combo.AddItem("Five Birds", "five");

                combo.ItemSelected += OnComboSelect;
            }

            {
                // Empty
                ComboBox combo = new ComboBox(layout);
                combo.Margin = Net.Margin.Five;
                combo.Width = 200;
            }

            {
                // Lots of things
                ComboBox combo = new ComboBox(layout);
                combo.Margin = Net.Margin.Five;
                combo.Width = 200;

                for (int i = 0; i < 500; i++)
                    combo.AddItem(String.Format("Option {0}", i));

                combo.ItemSelected += OnComboSelect;
            }

            {
                // Editable
                EditableComboBox combo = new EditableComboBox(layout);
                combo.Margin = Net.Margin.Five;
                combo.Width = 200;

                combo.AddItem("Option One", "one");
                combo.AddItem("Number Two", "two");
                combo.AddItem("Door Three", "three");
                combo.AddItem("Four Legs", "four");
                combo.AddItem("Five Birds", "five");

                combo.ItemSelected += (s, a) => UnitPrint(String.Format("ComboBox: OnComboSelect: {0}", combo.SelectedItem.Text)); ;

                combo.TextChanged += (s, a) => UnitPrint(String.Format("ComboBox: OnTextChanged: {0}", combo.Text));
                combo.SubmitPressed += (s, a) => UnitPrint(String.Format("ComboBox: OnSubmitPressed: {0}", combo.Text));
            }

            {
                HorizontalLayout hlayout = new HorizontalLayout(layout);
                {
                    // In-Code Item Change
                    ComboBox combo = new ComboBox(hlayout);
                    combo.Margin = Net.Margin.Five;
                    combo.Width = 200;

                    MenuItem Triangle = combo.AddItem("Triangle");
                    combo.AddItem("Red", "color");
                    combo.AddItem("Apple", "fruit");
                    combo.AddItem("Blue", "color");
                    combo.AddItem("Green", "color", 12);
                    combo.ItemSelected += OnComboSelect;

                    //Select by Menu Item
                    {
                        Button TriangleButton = new Button(hlayout);
                        TriangleButton.Text = "Triangle";
                        TriangleButton.Width = 100;
                        TriangleButton.Clicked += delegate (ControlBase sender, ClickedEventArgs args)
                        {
                            combo.SelectedItem = Triangle;
                        };
                    }

                    //Select by Text
                    {
                        Button TestBtn = new Button(hlayout);
                        TestBtn.Text = "Red";
                        TestBtn.Width = 100;
                        TestBtn.Clicked += delegate (ControlBase sender, ClickedEventArgs args)
                        {
                            combo.SelectByText("Red");
                        };
                    }

                    //Select by Name
                    {
                        Button TestBtn = new Button(hlayout);
                        TestBtn.Text = "Apple";
                        TestBtn.Width = 100;
                        TestBtn.Clicked += delegate (ControlBase sender, ClickedEventArgs args)
                        {
                            combo.SelectByName("fruit");
                        };
                    }

                    //Select by UserData
                    {
                        Button TestBtn = new Button(hlayout);
                        TestBtn.Text = "Green";
                        TestBtn.Width = 100;
                        TestBtn.Clicked += delegate (ControlBase sender, ClickedEventArgs args)
                        {
                            combo.SelectByUserData(12);
                        };
                    }
                }
            }
        }

        void OnComboSelect(ControlBase control, EventArgs args)
        {
            ComboBox combo = control as ComboBox;
            UnitPrint(String.Format("ComboBox: OnComboSelect: {0}", combo.SelectedItem.Text));
        }
    }
}