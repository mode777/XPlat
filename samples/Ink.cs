using System.Collections;
using System.Drawing;
using Coroutines;
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
        private CoroutineRunner runner;
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
            

            story = service.LoadStory("assets/crimescene.ink");

            layout = new Layout(vg);

            storyBox = new Panel();
            storyBox.Width = "66vw";
            storyBox.Style.Fill = "#ffffff";
            storyBox.Style.Shadow = new(0, 0, "5vh", "#00000088"); 
            storyBox.Padding = "5vh";

            optionBox = new Panel();
            optionBox.X = "66vw";
            optionBox.Width = "34vw";
            optionBox.Padding = "5vh";
            optionBox.Style.Fill = "#8899AA";

            layout.Children.Add(optionBox);
            layout.Children.Add(storyBox);
            layout.Arrange();

            platform.OnResize += (s,args) => layout.Arrange();

            runner = new CoroutineRunner();
            runner.Run(StoryCoroutine());
        }

        IEnumerator FadeInText(TextNode node, int frames){
            var col = node.Style.FontColor.Value;
            col.a = 0;
            node.Style.FontColor = col;
            float val = 0;
            float delta = 1.0f / frames;
            yield return null;
            while(frames > 0){
                val += delta;
                col.a = val;
                node.Style.FontColor = col;
                frames--;
                yield return null;
            }
        }

        IEnumerator StoryCoroutine()
        {
            while(true){

                if (story.canContinue)
                {
                    storyBox.Children.Clear();
                    optionBox.Children.Clear();
                    while (story.canContinue)
                    {
                        var n = new TextNode
                        {
                            Text = story.Continue(),
                            Size = "5vh"
                        };
                        n.Style.FontColor = "#00000000";
                        storyBox.Children.Add(n);
                        layout.Arrange();
                        yield return FadeInText(n, 60);
                    }
                }
                if (story.currentChoices.Count > 0 && !optionBox.Children.Any())
                {
                    foreach (var c in story.currentChoices)
                    {
                        var n = new TextNode();
                        n.Text = c.text;
                        n.Style.FontColor = "#ffffff00";
                        n.HoverStyle.FontColor = "#ffff00";
                        n.TextAlign = NVGalign.NVG_ALIGN_CENTER;
                        n.OnClick += (s, args) => {
                            story.ChooseChoiceIndex(c.index);
                        };
                        optionBox.Children.Add(n);
                        layout.Arrange();
                        yield return FadeInText(n, 15);
                    }
                }
                yield return null;
            }
        }

        public void Update()
        {
            runner.Update(0);
            
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