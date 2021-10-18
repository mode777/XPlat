using System.Drawing;
using GLES2;
using Ink.Runtime;
using NanoVGDotNet;

namespace net6test.samples
{
    public class InkApp : ISdlApp
    {
        private readonly InkService service;
        private NVGcontext vg;
        private int font;
        private Story story;
        private List<string> paragraphs;
        private List<Choice> choices;
        private List<float[]> choiceLocations;
        private readonly IPlatformInfo platform;
        private SizeF scale;
        private float textScroll = 0;

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
            font = vg.CreateFont("sans", "assets/Roboto-Regular.ttf");

            story = service.LoadStory("assets/test.ink");
            paragraphs = new List<string>();
            choices = new List<Choice>();
        }

        public void Update()
        {
            if(story.canContinue){
                paragraphs.Add(story.Continue());
            }
            if(story.currentChoices.Count > 0){
                choices.Clear();
                choices.AddRange(story.currentChoices);
            }

            Draw();
        }

        private void Draw()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
            var windowSize = platform.WindowSize;
            var rendererSize = platform.RendererSize;
            scale.Width = (float)rendererSize.Width / 100.0f;
            scale.Height = (float)rendererSize.Height / 100.0f;
            vg.BeginFrame(rendererSize.Width, rendererSize.Height, 1);
            //vg.Scale(scale.Width, scale.Height);
            
            var leftPanelW = rendererSize.Width*0.75f;
            var textMargin = rendererSize.Height*0.1f;
            var fontSize = rendererSize.Height * 0.05f;
            LeftPanel(leftPanelW, rendererSize.Height, rendererSize.Width*0.025f);
            DrawChoices(fontSize, leftPanelW + textMargin, textMargin, rendererSize.Width - leftPanelW - textMargin);
            DrawParagraphs(fontSize, textMargin, textMargin-textScroll, leftPanelW - textMargin*2);

            vg.EndFrame();
        }

        private void DrawParagraphs(float fontSize, float x, float y, float w)
        {
            vg.FontSize(fontSize);
            var yOffset = 0f;
            vg.FillColor(vg.RGBA(0,0,0,255));
            float[] bounds = new float[4];
            foreach (var text in paragraphs)
            {
                vg.TextBoxBounds(x,y+yOffset,w,text, bounds);
                vg.TextBox(x, y+yOffset, w, text);
                yOffset += (bounds[3]-bounds[1] + fontSize);
            }
            var overlap = bounds[3] - (float)platform.RendererSize.Height;
            if(overlap > 0) textScroll += overlap + scale.Height * .5f;
        }

        private void DrawChoices(float fontSize, float x, float y, float w)
        {
            vg.FontSize(fontSize);
            var yOffset = 0f;
            float[] bounds = new float[4];
            foreach (var choice in choices)
            {
                vg.TextBoxBounds(x,y+yOffset,w,choice.text, bounds);
                var rect = new RectangleF(bounds[0],bounds[1],bounds[2]-bounds[0],bounds[3]-bounds[1]);
                var normalized = new PointF(platform.MousePosition.X * 2, platform.MousePosition.Y * 2);
                if(rect.Contains(normalized)){
                    vg.BeginPath();
                    vg.RoundedRect(rect.X, rect.Y, rect.Width, rect.Height, 1*scale.Height);
                    vg.FillColor(vg.RGBA(0,0,0,128));
                    vg.Fill();
                    if(platform.MouseClicked) story.ChooseChoiceIndex(choice.index);
                }
                vg.FillColor(vg.RGBA(255,255,255,255));
                vg.TextBox(x, y+yOffset, w, choice.text);
                yOffset += (bounds[3]-bounds[1] + fontSize / 2);
            }

        }

        private void LeftPanel(float w, float h, float shadowW)
        {
            vg.BeginPath();
            vg.Rect(0,0,w,h);
            vg.FillColor(vg.RGBA(255,255,255,255));
            vg.Fill();

            vg.BeginPath();
            vg.Rect(w, 0, shadowW, h);
            vg.FillPaint(vg.LinearGradient(w, 0, w + shadowW, 0, vg.RGBA(0,0,0,64), vg.RGBA(0,0,0,0)));
            vg.Fill();
        }


    }
}