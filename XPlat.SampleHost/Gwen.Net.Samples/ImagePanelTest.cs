using System;
using Gwen.Net;
using Gwen.Net.Control;

namespace Gwen.Net.Tests.Components
{
    [UnitTest(Category = "Non-Interactive", Order = 105)]
    public class ImagePanelTest : GUnit
    {
        public ImagePanelTest(ControlBase parent)
            : base(parent)
        {
            /* Normal */
            {
                ImagePanel img = new ImagePanel(this)
                {
                    Margin = Margin.Five,
                    Dock = Dock.Top,
                    Size = new Size(100, 100),
                    ImageName = "assets/ui/gwen.png"
                };
            }


            /* Missing */
            {
                ImagePanel img = new ImagePanel(this)
                {
                    Margin = Margin.Five,
                    Dock = Dock.Top,
                    Size = new Size(100, 100),
                    ImageName = "missingimage.png"
                };
            }

            /* Clicked */
            {
                ImagePanel img = new ImagePanel(this)
                {
                    Margin = Margin.Five,
                    Dock = Dock.Top,
                    Size = new Size(100, 100),
                    ImageName = "test16.png"
                };
                img.Clicked += Image_Clicked;
            }
        }

        void Image_Clicked(ControlBase control, EventArgs args)
        {
            UnitPrint("Image: Clicked");
        }
    }
}