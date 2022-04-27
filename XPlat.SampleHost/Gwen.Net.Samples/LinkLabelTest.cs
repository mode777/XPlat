using Gwen.Net;
using Gwen.Net.Control;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Non-Interactive", Order = 101)]
    public class LinkLabelTest : GUnit
    {
        private readonly Font font1;
        private readonly Font fontHover1;

        public LinkLabelTest(ControlBase parent)
            : base(parent)
        {
            {
                LinkLabel label = new LinkLabel(this);
                label.Dock = Dock.Top;
                label.HoverColor = new Color(255, 255, 255, 255);
                label.Text = "Link Label (default font)";
                label.Link = "Test Link";
                label.LinkClicked += OnLinkClicked;
            }
            {
                font1 = new Font(Skin.Renderer, "sans", 25);
                //font1 = new Font(Skin.Renderer, "Comic Sans MS", 25);
                //fontHover1 = new Font(Skin.Renderer, "Comic Sans MS", 25);
                fontHover1 = new Font(Skin.Renderer, "sans", 25);
                fontHover1.Underline = true;

                LinkLabel label = new LinkLabel(this);
                label.Dock = Dock.Top;
                label.Font = font1;
                label.HoverFont = fontHover1;
                label.TextColor = new Color(255, 0, 80, 205);
                label.HoverColor = new Color(255, 0, 100, 255);
                label.Text = "Custom Font (Comic Sans 25)";
                label.Link = "Custom Font Link";
                label.LinkClicked += OnLinkClicked;
            }
        }

        public override void Dispose()
        {
            font1.Dispose();
            fontHover1.Dispose();
            base.Dispose();
        }

        private void OnLinkClicked(ControlBase control, LinkClickedEventArgs args)
        {
            UnitPrint("Link Clicked: " + args.Link);
        }
    }
}