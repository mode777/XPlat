using System;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Property;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Containers", Order = 303)]
    public class PropertiesTest : GUnit
    {
        public PropertiesTest(ControlBase parent)
            : base(parent)
        {
            {
                Properties props = new Properties(this);
                props.Dock = Dock.Top;
                props.Width = 300;
                props.ValueChanged += OnChanged;

                {
                    {
                        PropertyRow pRow = props.Add("First Name");
                    }

                    props.Add("Middle Name");
                    props.Add("Last Name");
                }
            }

            {
                PropertyTree ptree = new PropertyTree(this);
                ptree.Dock = Dock.Top;
                ptree.Width = 300;
                ptree.AutoSizeToContent = true;

                {
                    Properties props = ptree.Add("Item One");
                    props.ValueChanged += OnChanged;

                    props.Add("Middle Name");
                    props.Add("Last Name");
                    props.Add("Four");
                }

                {
                    Properties props = ptree.Add("Item Two");
                    props.ValueChanged += OnChanged;

                    props.Add("More Items");
                    props.Add("Bacon", new CheckProperty(props), "1");
                    props.Add("To Fill");
                    props.Add("Color", new ColorProperty(props), "255 0 0");
                    props.Add("Out Here");
                }

                ptree.ExpandAll();
            }
        }

        void OnChanged(ControlBase control, EventArgs args)
        {
            PropertyRow row = control as PropertyRow;
            UnitPrint(String.Format("Property changed: {0}", row.Value));
        }
    }
}