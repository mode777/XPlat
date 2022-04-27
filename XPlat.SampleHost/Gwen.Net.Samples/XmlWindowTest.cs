using System;
using Gwen.Net;
using Gwen.Net.Xml;
using Gwen.Net.Control;
using Gwen.Net.Control.Layout;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Xml", Order = 600)]
    public class XmlWindowTest : GUnit
    {
        public XmlWindowTest(ControlBase parent)
            : base(parent)
        {
            HorizontalLayout layout = new HorizontalLayout(this);
            layout.VerticalAlignment = VerticalAlignment.Top;

            Button button1 = new Button(layout);
            button1.Text = "Open a Window (method)";
            button1.Clicked += OpenMethodWindow;

            Button button2 = new Button(layout);
            button2.Text = "Open a Window (interface)";
            button2.Clicked += OpenInterfaceWindow;
        }

        private class MethodTestComponent : Component
        {
            public MethodTestComponent(GUnit unit, string xml)
                : base(unit, new XmlStringSource(xml))
            {
                m_unit = unit;
            }

            public void OnButtonClicked(ControlBase sender, ClickedEventArgs args)
            {
                m_unit.UnitPrint(sender.Name + ": Clicked");
            }

            public void OnItemSelected(ControlBase sender, ItemSelectedEventArgs args)
            {
                m_unit.UnitPrint(sender.Name + ": ItemSelected " + ((MenuItem)args.SelectedItem).Text);
            }

            public void OnSelectionChanged(ControlBase sender, ItemSelectedEventArgs args)
            {
                m_unit.UnitPrint(sender.Name + ": SelectionChanged " + ((LabeledRadioButton)args.SelectedItem).Text);
            }

            public void OnValueChanged(ControlBase sender, EventArgs args)
            {
                float value = 0.0f;
                if (sender is NumericUpDown)
                    value = ((NumericUpDown)sender).Value;
                else if (sender is VerticalSlider)
                    value = ((VerticalSlider)sender).Value;
                else if (sender is HorizontalSlider)
                    value = ((HorizontalSlider)sender).Value;

                m_unit.UnitPrint(sender.Name + ": ValueChanged " + value);
            }

            public void OnTextChanged(ControlBase sender, EventArgs args)
            {
                if (sender is MultilineTextBox)
                    m_unit.UnitPrint(sender.Name + ": TextChanged " + ((MultilineTextBox)sender).Text);
                else if (sender is TextBox)
                    m_unit.UnitPrint(sender.Name + ": TextChanged " + ((TextBox)sender).Text);
            }

            public void OnSubmitPressed(ControlBase sender, EventArgs args)
            {
                if (sender is TextBox)
                    m_unit.UnitPrint(sender.Name + ": SubmitPressed " + ((TextBox)sender).Text);
            }

            public void OnCheckChanged(ControlBase sender, EventArgs args)
            {
                if (sender is CheckBox)
                    m_unit.UnitPrint(sender.Name + ": CheckChanged " + ((CheckBox)sender).IsChecked);
                else if (sender is LabeledCheckBox)
                    m_unit.UnitPrint(sender.Name + ": CheckChanged " + ((LabeledCheckBox)sender).IsChecked);
            }

            public void OnRowSelected(ControlBase sender, ItemSelectedEventArgs args)
            {
                m_unit.UnitPrint(sender.Name + ": RowSelected " + ((ListBoxRow)((ItemSelectedEventArgs)args).SelectedItem).Text);
            }

            public void OnSelected(ControlBase sender, EventArgs args)
            {
                m_unit.UnitPrint(((TreeNode)sender).TreeControl.Name + ": Selected " + ((TreeNode)sender).Text);
            }

            public void OnClosed(ControlBase sender, EventArgs args)
            {
                m_unit.UnitPrint(sender.Name + ": Closed ");
            }

            private GUnit m_unit;
        }

        private class InterfaceTestComponent : Component
        {
            public InterfaceTestComponent(GUnit unit, string xml)
                : base(unit, new XmlStringSource(xml))
            {
                m_unit = unit;
            }

            public override bool HandleEvent(string eventName, string handlerName, ControlBase sender, System.EventArgs args)
            {
                if (handlerName == "OnButtonClicked")
                {
                    m_unit.UnitPrint(sender.Name + ": Clicked");
                    return true;
                }
                else if (handlerName == "OnItemSelected")
                {
                    m_unit.UnitPrint(sender.Name + ": ItemSelected " + ((MenuItem)((ItemSelectedEventArgs)args).SelectedItem).Text);
                    return true;
                }
                else if (handlerName == "OnSelectionChanged")
                {
                    m_unit.UnitPrint(sender.Name + ": SelectionChanged " + ((LabeledRadioButton)((ItemSelectedEventArgs)args).SelectedItem).Text);
                    return true;
                }
                else if (handlerName == "OnValueChanged")
                {
                    float value = 0.0f;
                    if (sender is NumericUpDown)
                        value = ((NumericUpDown)sender).Value;
                    else if (sender is VerticalSlider)
                        value = ((VerticalSlider)sender).Value;
                    else if (sender is HorizontalSlider)
                        value = ((HorizontalSlider)sender).Value;

                    m_unit.UnitPrint(sender.Name + ": ValueChanged " + value);
                    return true;
                }
                else if (handlerName == "OnTextChanged")
                {
                    if (sender is TextBox)
                        m_unit.UnitPrint(sender.Name + ": TextChanged " + ((TextBox)sender).Text);
                    else if (sender is MultilineTextBox)
                        m_unit.UnitPrint(sender.Name + ": TextChanged " + ((MultilineTextBox)sender).Text);
                    return true;
                }
                else if (handlerName == "OnSubmitPressed")
                {
                    m_unit.UnitPrint(sender.Name + ": SubmitPressed " + ((TextBox)sender).Text);
                    return true;
                }
                else if (handlerName == "OnCheckChanged")
                {
                    if (sender is CheckBox)
                        m_unit.UnitPrint(sender.Name + ": CheckChanged " + ((CheckBox)sender).IsChecked);
                    else if (sender is LabeledCheckBox)
                        m_unit.UnitPrint(sender.Name + ": CheckChanged " + ((LabeledCheckBox)sender).IsChecked);
                    return true;
                }
                else if (handlerName == "OnRowSelected")
                {
                    m_unit.UnitPrint(sender.Name + ": RowSelected " + ((ListBoxRow)((ItemSelectedEventArgs)args).SelectedItem).Text);
                    return true;
                }
                else if (handlerName == "OnSelected")
                {
                    m_unit.UnitPrint(((TreeNode)sender).TreeControl.Name + ": Selected " + ((TreeNode)sender).Text);
                    return true;
                }
                else if (handlerName == "OnClosed")
                {
                    m_unit.UnitPrint(sender.Name + ": Closed ");
                    return true;
                }
                else
                {
                    return false;
                }
            }

            private GUnit m_unit;
        }

        void OpenMethodWindow(ControlBase control, EventArgs args)
        {
            new MethodTestComponent(this, m_xml);
        }

        void OpenInterfaceWindow(ControlBase control, EventArgs args)
        {
            new InterfaceTestComponent(this, m_xml);
        }

        private readonly string m_xml = @"<?xml version='1.0' encoding='UTF-8'?>
			<Window Name='Window' Position='100, 100' Size='500, 500' MinimumSize='200,200' Padding='6' Title='Xml Window'>
				<VerticalSplitter>
					<VerticalLayout>
						<Label Text='Label' Font='Arial; 20; Italic; Bold' />
						<Button Name='Button' Text='Button' Clicked='OnButtonClicked' />
						<NumericUpDown Name='NumericUpDown' Min='20' Max='80' Step='10' Value='50' ValueChanged='OnValueChanged' />
						<TextBox Name='TextBox' SelectAllOnFocus='True' TextChanged='OnTextChanged' SubmitPressed='OnSubmitPressed' />
						<TextBoxNumeric Name='TextBoxNumeric' TextChanged='OnTextChanged' SubmitPressed='OnSubmitPressed' />
						<TextBoxPassword Name='TextBoxPassword' TextChanged='OnTextChanged' SubmitPressed='OnSubmitPressed' />
						<MultilineTextBox Height='100' Name='MultilineTextBox' TextChanged='OnTextChanged' />
						<LabeledCheckBox Name='CheckBox' Text='LabeledCheckBox' CheckChanged='OnCheckChanged' />
						<ComboBox Name='ComboBox' ItemSelected='OnItemSelected'>
							<Option Text='Item 1' />
							<Option Text='Item 2' />
							<Option Text='Item 3' />
							<Option Text='Item 4' />
						</ComboBox>
						<RadioButton Name='RadioButtonGroup' SelectionChanged='OnSelectionChanged'>
							<Option Name='Button1' Text='Button 1' />
							<Option Name='Button2' Text='Button 2' />
							<Option Name='Button3' Text='Button 3' />
						</RadioButton>
					</VerticalLayout>
					<VerticalLayout>
						<ListBox Name='ListBox' ColumnCount='3' AutoSizeToContent='True' RowSelected='OnRowSelected'>
							<Row Name='Row1' Text='Row 1'>
								<Column Text='Col 1' />
								<Column Text='Col 2' />
							</Row>
							<Row Name='Row2' Text='Row 2'>
								<Column Text='Col 1' />
								<Column Text='Col 2' />
							</Row>
							<Row Name='Row3' Text='Row 3'>
								<Column Text='Col 1' />
								<Column Text='Col 2' />
							</Row>
						</ListBox>
						<TreeControl Name='TreeControl' Height='150' Selected='OnSelected'>
							<TreeNode Name='Node1' Text='Node 1'>
								<TreeNode Name='Node2' Text='Node 2' />
								<TreeNode Name='Node3' Text='Node 3' />
							</TreeNode>
							<TreeNode Name='Node4' Text='Node 4' />
							<TreeNode Name='Node5' Text='Node 5'>
								<TreeNode Name='Node6' Text='Node 6'>
									<TreeNode Name='Node7' Text='Node 7' />
									<TreeNode Name='Node8' Text='Node 8' />
								</TreeNode>
								<TreeNode Name='Node9' Text='Node 9' />
							</TreeNode>
						</TreeControl>
						<TabControl AllowReorder='True'>
							<TabPage Text='Page 1'>
								<Label Text='Page 1 Label 1' />
							</TabPage>
							<TabPage Text='Page 2'>
								<Button Size='50, 25' />
							</TabPage>
							<TabPage Text='Page 3'>
								<VerticalLayout>
									<Label Margin='2' Text='Page 3 Label 1' />
									<Label Margin='2' Text='Page 3 Label 2' />
								</VerticalLayout>
							</TabPage>
						</TabControl>
					</VerticalLayout>
				</VerticalSplitter>
			</Window>
";
    }
}