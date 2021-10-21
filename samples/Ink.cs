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
        private Panel textBox;
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

            textBox = new Panel {
                Rect = new(0, 0, "66vw", "100vh"),
                Fill = "#ffffff",
                Shadow = new(0, 0, "5vh", "#00000088"),
                Padding = (Quantity)5f
            };

            text = new TextNode
            {
                Text = "This is some expanding tex that probably overflows",
                Size = "7vh"
            };
            textBox.Children.Add(text);
            text = new TextNode
            {
                Text = "Microsoft.Hosting.Lifetime: Information: Application started. Press Ctrl+C to shut down.\nMicrosoft.Hosting.Lifetime: Information: Hosting environment: Production",
                Size = "7vh"
            };
            textBox.Children.Add(text);

        }

        public void Update()
        {
            // if(story.canContinue){
            //     paragraphs.Add(story.Continue());
            // }
            // if(story.currentChoices.Count > 0){
            //     choices.Clear();
            //     choices.AddRange(story.currentChoices);
            // }

            //textBox.UpdateBounds(platform);
            //text.FitInto(textBox.PaddingBounds, vg, platform);

            textBox.Arrange(new() { Vg = vg });
            textBox.Update(vg);
            //text.UpdateBounds(platform);
            Draw();
        }

        private void Draw()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
            vg.BeginFrame(platform.RendererSize.Width, platform.RendererSize.Height, 1);
            
            textBox.Draw(vg);
            // //vg.Scale(scale.Width, scale.Height);
            
            // var leftPanelW = rendererSize.Width*0.75f;
            // // var textMargin = rendererSize.Height*0.1f;
            // // var fontSize = rendererSize.Height * 0.05f;
            // LeftPanel(leftPanelW, rendererSize.Height, rendererSize.Width*0.025f);
            // // DrawChoices(fontSize, leftPanelW + textMargin, textMargin, rendererSize.Width - leftPanelW - textMargin);
            // // DrawParagraphs(fontSize, textMargin, textMargin-textScroll, leftPanelW - textMargin*2);

            // var shadowPaint = vg.BoxGradient(100, 100, 100,100, 10, 10, "#000000ff", "#00000000");
            // vg.BeginPath();
            // vg.Rect(90, 90, 120, 120);
            // //NanoVG.nvgRoundedRect(vg, x,y, w,h, cornerRadius);
            // //NanoVG.nvgPathWinding(vg, (int)NVGsolidity.NVG_HOLE);
            // vg.FillPaint(shadowPaint);
            // //vg.FillColor("#000000ff");
            // vg.Fill();

            vg.EndFrame();
        }

        private void DrawParagraphs(float fontSize, float x, float y, float w)
        {
            // vg.FontSize(fontSize);
            // var yOffset = 0f;
            // vg.FillColor(vg.RGBA(0,0,0,255));
            // float[] bounds = new float[4];
            // foreach (var text in paragraphs)
            // {
            //     vg.TextBoxBounds(x,y+yOffset,w,text, bounds);
            //     vg.TextBox(x, y+yOffset, w, text);
            //     yOffset += (bounds[3]-bounds[1] + fontSize);
            // }
            // var overlap = bounds[3] - (float)platform.RendererSize.Height;
            // if(overlap > 0) textScroll += overlap + scale.Height * .5f;
        }

        private void DrawChoices(float fontSize, float x, float y, float w)
        {
            // vg.FontSize(fontSize);
            // var yOffset = 0f;
            // float[] bounds = new float[4];
            // foreach (var choice in choices)
            // {
            //     vg.TextBoxBounds(x,y+yOffset,w,choice.text, bounds);
            //     var rect = new RectangleF(bounds[0],bounds[1],bounds[2]-bounds[0],bounds[3]-bounds[1]);
            //     var normalized = new PointF(platform.MousePosition.X, platform.MousePosition.Y);
            //     if(rect.Contains(normalized)){
            //         vg.BeginPath();
            //         vg.RoundedRect(rect.X, rect.Y, rect.Width, rect.Height, 1*scale.Height);
            //         vg.FillColor(vg.RGBA(0,0,0,128));
            //         vg.Fill();
            //         if(platform.MouseClicked) story.ChooseChoiceIndex(choice.index);
            //     }
            //     vg.FillColor(vg.RGBA(255,255,255,255));
            //     vg.TextBox(x, y+yOffset, w, choice.text);
            //     yOffset += (bounds[3]-bounds[1] + fontSize / 2);
            // }

        }

        


    }
}