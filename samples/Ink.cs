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
        private int img;
        private Story story;
        private Layout layout;
        private Panel panel;
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
            vg.CreateFont("serif", "assets/Merriweather-Regular.ttf");
            img = vg.CreateImage("assets/bamberg.png", 0);
            
            story = service.LoadStory("assets/test.ink");

            layout = new Layout(vg);
            panel = new Panel();
            panel.Style.Fill = "#000000aa";
            panel.Padding = "3vh 6vh";
            panel.Width = "33vw";
            panel.X = "60vw";
            panel.Spacing = "2vh";
            panel.Style.Shadow = new Shadow(0,0,"1vh", "#000000");
            layout.Children.Add(panel);
            layout.Arrange();

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

        TextNode CreateParagraphNode(string txt){
            var n = new TextNode
            {
                Text = txt,
                Font = "serif",
                Size = "2.5vh"
            };
            n.Style.FontColor = "#ffffff00";
            return n;
        }

        TextNode CreateOptionNode(int idx, string txt, Action<int> cb){
            var n = new TextNode();
            n.Font = "serif";
            n.Size = "2.5vh";
            n.Text = idx + " - " + txt;
            n.Style.FontColor = "#ff220000";
            n.HoverStyle.FontColor = "#ffffff";
            n.TextAlign = NVGalign.NVG_ALIGN_LEFT;
            n.OnClick += (s, args) => cb(idx);
            return n;
        }

        IEnumerator StoryCoroutine()
        {
            while(true){

                if (story.canContinue)
                {
                    panel.Children.Clear();
                    while (story.canContinue)
                    {
                        var txt = story.Continue();
                        if(!String.IsNullOrWhiteSpace(txt)){
                            var n = CreateParagraphNode(txt);
                            panel.Children.Add(n);
                            yield return FadeInText(n, 60);
                        }
                    }
                }
                if (story.currentChoices.Count > 0)
                {
                    var index = -1;
                    var p = new Panel();
                    p.Id = "optionPanel";
                    panel.Children.Add(p);
                    foreach (var c in story.currentChoices)
                    {
                        var n = CreateOptionNode(c.index, c.text, i => index = i);
                        p.Children.Add(n);
                        yield return FadeInText(n, 15);
                    }
                    while(index == -1){
                        yield return null;
                    }
                    story.ChooseChoiceIndex(index);
                }
                yield return null;
            }
        }

        public void Update()
        {
            runner.Update(0);
            layout.Arrange();
            
            //text.UpdateBounds(platform);
            layout.Update();
            Draw();
        }

        private void Draw()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
            
            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);

            vg.BeginPath();
            vg.Rect(0,0,(Quantity)"100vw", (Quantity)"100vh");
            vg.FillColor("#F6D7A7");
            vg.Fill();

            vg.BeginPath();
            vg.Rect(0,0,(Quantity)"100vw",(Quantity)"100vh");
            var p = vg.ImagePattern((Quantity)"-10vw",(Quantity)"0vh",(Quantity)"111vw",(Quantity)"101vh",0,img,1);
            vg.FillPaint(p);
            vg.Fill();

            vg.EndFrame();

            layout.Draw();
        }
    }
}