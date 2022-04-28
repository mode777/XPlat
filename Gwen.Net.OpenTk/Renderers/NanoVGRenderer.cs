using System;
using System.IO;
using Gwen.Net.Renderer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Core;
using XPlat.NanoVg;

namespace Gwen.Net.OpenTk.Renderers
{
	public class NanoVGRenderer : RendererBase
	{
        private readonly NVGcontext vg;
        private readonly IPlatform platform;

        public NanoVGRenderer(NVGcontext vg, IPlatform platform)
		{
            this.vg = vg;
            this.platform = platform;
        }

        public override void Begin()
        {
            vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);
        }

        private NVGcolor VGcolor => vg.RGBA(DrawColor.A, DrawColor.G, DrawColor.B, DrawColor.A);

        public override void DrawFilledRect(Rectangle rect)
        {
            vg.BeginPath();
            vg.Rect(rect.X + RenderOffset.X, rect.Y+RenderOffset.Y, rect.Width, rect.Height);
            vg.FillColor(VGcolor);
            vg.Fill();
        }

        public override void DrawLine(int x, int y, int a, int b)
        {
            vg.BeginPath();
            vg.MoveTo(x+RenderOffset.X, y+RenderOffset.Y);
            vg.LineTo(a+RenderOffset.X, b+RenderOffset.Y);
            vg.StrokeWidth(1);
            vg.StrokeColor(VGcolor);
            vg.Stroke();
        }

        public override void DrawLinedRect(Rectangle rect)
        {
            vg.BeginPath();
            vg.Rect(rect.X + RenderOffset.X, rect.Y + RenderOffset.Y, rect.Width, rect.Height);
            vg.StrokeWidth(1);
            vg.StrokeColor(VGcolor);
            vg.Stroke();
        }

        public override void DrawShavedCornerRect(Rectangle rect, bool slight = false)
        {
            vg.BeginPath();
            vg.RoundedRect(rect.X+RenderOffset.X, rect.Y+RenderOffset.Y, rect.Width, rect.Height, slight ? 5 : 10);
            vg.StrokeWidth(1);
            vg.StrokeColor(VGcolor);
            vg.Stroke();
        }

        Random r = new Random();
        NVGcolor RandColor => vg.RGBA((byte)r.Next(255),(byte)r.Next(255),(byte)r.Next(255),255);

        public override void DrawTexturedRect(Texture t, Rectangle targetRect, float u1 = 0, float v1 = 0, float u2 = 1, float v2 = 1)
        {
            if(t.Failed == true){
                DrawMissingImage(targetRect);
                return;
            } 
            int w = t.Width, h = t.Height;
            var x = targetRect.X + RenderOffset.X;
            var y = targetRect.Y + RenderOffset.Y;

            vg.BeginPath();
            vg.Rect(x,y,targetRect.Width,targetRect.Height);

            var ix = u1 * w;
            var ix2 = u2 * w;
            var iy = v1 * h;
            var iy2 = v2 * h;
            var sx = targetRect.Width / (float)(ix2 - ix);
            var sy = targetRect.Height / (float)(iy2 - iy);

            var p = vg.ImagePattern(x-(ix*sx),y-(iy*sy),w*sx,h*sy,0,((TextureRendererData)t.RendererData).NvgHandle,1);
            vg.FillPaint(p);
            vg.Fill();

            // vg.BeginPath();
            // int x = RenderOffset.X + targetRect.X, y = RenderOffset.Y + targetRect.Y;
            // vg.Rect(x, y, targetRect.Width, targetRect.Height);

            // var paint = vg.ImagePattern(x - u1*w,y - v1*h,(u2-u1)/w,(v2-v1)/h, 0, ((TextureRendererData)t.RendererData).NvgHandle, 1);
            // vg.FillPaint(paint);
            // //vg.FillColor(RandColor);

            // vg.Fill();
        }

        public override void End()
        {
            vg.EndFrame();
        }

        public override void EndClip()
        {
            vg.ResetScissor();
        }

        public override void FreeFont(Font font)
        {
            // TODO?
        }

        public override void FreeTexture(Texture t)
        {
            vg.DeleteImage(((TextureRendererData)t.RendererData).NvgHandle);
        }

        public override FontMetrics GetFontMetrics(Font font)
        {
            vg.FontFace("sans");
            vg.FontSize(font.Size);
            float asc = 0, desc = 0, line = 0;
            vg.TextMetrics(ref asc, ref desc, ref line);
            var fm = new FontMetrics(emHeightPixels: line, ascentPixels: asc, descentPixels: desc, cellHeightPixels: 0, internalLeadingPixels: 0, lineSpacingPixels: line, externalLeadingPixels: 0);
            return fm;
        }

        public override bool LoadFont(Font font)
        {
            throw new NotImplementedException();
        }

        private class TextureRendererData
        {
            public Image<Rgba32> Pixels;
            public int NvgHandle;
        }

        public override void LoadTexture(Texture t)
        {
            if(File.Exists(t.Name)){
                var data = new TextureRendererData();
                data.Pixels = Image.Load<Rgba32>(t.Name);
                data.NvgHandle = vg.CreateImage(data.Pixels, 0);
                t.Width = data.Pixels.Width;
                t.Height = data.Pixels.Height;
                t.RendererData = data;
            } else {
                t.Failed = true;
            }
        }

        public override void LoadTextureRaw(Texture t, byte[] pixelData)
        {
            var data = new TextureRendererData();
            data.Pixels = Image.Load<Rgba32>(pixelData);
            data.NvgHandle = vg.CreateImage(data.Pixels, 0);
            t.Width = data.Pixels.Width;
            t.Height = data.Pixels.Height;
            t.RendererData = data;
        }

        public override void LoadTextureStream(Texture t, Stream stream)
        {
            var data = new TextureRendererData();
            data.Pixels = Image.Load<Rgba32>(stream);
            data.NvgHandle = vg.CreateImage(data.Pixels, 0);
            t.Width = data.Pixels.Width;
            t.Height = data.Pixels.Height;
            t.RendererData = data;
        }

        private float[] bounds = new float[4];

        public override Size MeasureText(Font font, string text)
        {
            vg.FontFace("sans");
            vg.FontSize(font.Size);
            vg.TextAlign((int)VerticalAlignment.Bottom | (int)HorizontalAlignment.Left);
            vg.TextBounds(0, 0, text, bounds);
            return new Size((int)MathF.Abs(bounds[0]- bounds[2]), (int)MathF.Abs((bounds[1]-bounds[3])));
        }

        public override void RenderText(Font font, Point position, string text)
        {
            vg.FontFace("sans");
            vg.FontSize(font.Size);
            vg.FillColor(vg.RGBA(DrawColor.R,DrawColor.G,DrawColor.B,DrawColor.A));
            vg.TextAlign((int)VerticalAlignment.Bottom | (int)HorizontalAlignment.Left);
            vg.Text(position.X + RenderOffset.X, position.Y+RenderOffset.Y + font.FontMetrics.Baseline, text);

            //System.Console.WriteLine($"{position.X + RenderOffset.X},{position.Y+RenderOffset.Y},{text}");
        }

        public override void StartClip()
        {
            vg.Scissor(ClipRegion.X, ClipRegion.Y, ClipRegion.Width, ClipRegion.Height);
        }

        protected override void OnScaleChanged(float oldScale)
        {
            // TODO?
        }

        public override Color PixelColor(Texture texture, uint x, uint y, Color defaultColor)
        {
            var data = (TextureRendererData)texture.RendererData;
            if(data.Pixels.TryGetSinglePixelSpan(out var span))
            {
                var pixel = span[(int)(y * data.Pixels.Width + x)];
                return new Color(pixel.R, pixel.G, pixel.B, pixel.A);
            }
            return defaultColor;
        }
    }
}

