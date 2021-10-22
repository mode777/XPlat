using System.Drawing;
using GLES2;
using Ink.Runtime;
using NanoVGDotNet;
using net6test.UI;

namespace net6test.samples
{

    public class InkApp : ISdlApp
    {
        private readonly InkService service;
        private NVGcontext vg;
        private Story story;
        private Layout layout;
        private Panel storyBox;
        private Panel optionBox;
        private TextNode text;
        private readonly IPlatformInfo platform;

        public InkApp(InkService service, IPlatformInfo platform)
        {
            this.platform = platform;
            this.service = service;
        }

        public void Init()
        {
            vg = new NVGcontext();
            GlNanoVG.nvgCreateGL(ref vg, (int)NVGcreateFlags.NVG_ANTIALIAS |
                        (int)NVGcreateFlags.NVG_STENCIL_STROKES);
            vg.CreateFont("sans", "assets/Roboto-Regular.ttf");
            

            story = service.LoadStory("assets/test.ink");

            layout = new Layout(vg);

            storyBox = new Panel {
                Rect = new(0, 0, "66vw", "100vh"),
                Fill = "#ffffff",
                Shadow = new(0, 0, "5vh", "#00000088"),
                Padding = "5vh"
            };

            optionBox = new Panel {
                Rect = new("66vw",0,"34vw","100vh"),
                Padding = "10vh 5vh",
                Fill = "#8899AA"
            };

            layout.Children.Add(optionBox);
            layout.Children.Add(storyBox);
            layout.Arrange();
        }

        void UpdateStory()
        {
            if (story.canContinue)
            {
                storyBox.Children.Clear();
                optionBox.Children.Clear();
                while (story.canContinue)
                {
                    storyBox.Children.Add(new TextNode
                    {
                        Text = story.Continue(),
                        Size = "5vh"
                    });
                    //layout.Arrange();
                }
            }
            if (story.currentChoices.Count > 0 && !optionBox.Children.Any())
            {
                foreach (var c in story.currentChoices)
                {
                    var n = new TextNode
                    {
                        Text = c.text,
                        Color = "#ffffff",
                        TextAlign = NVGalign.NVG_ALIGN_CENTER
                    };
                    n.OnClick += (s, args) => {
                        story.ChooseChoiceIndex(c.index);
                    };
                    optionBox.Children.Add(n);
                }
                layout.Arrange();
            }

        }

        public void Update()
        {
            UpdateStory();
            
            //text.UpdateBounds(platform);
            layout.Update();
            Draw();
        }

        private void Draw()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
            
            layout.Draw();
        }
    }
}