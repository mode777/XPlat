using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{
    public static class RectangleFExtensions {
        public static RectangleF Expand(this RectangleF r, float offset) => new RectangleF(r.X + offset, r.Y + offset, r.Width + offset, r.Height + offset);
        public static RectangleF Scale(this RectangleF r, float factor) => new RectangleF(r.X * factor, r.Y * factor, r.Width * factor, r.Height * factor);
    }

    public class Shadow 
    {
        public Shadow(SizeF offset, float size, NVGcolor color)
        {
            Offset = offset;
            Size = size;
            Color = color;
        }

        public SizeF Offset { get; set; }
        public float Size { get; set; }
        public NVGcolor Color { get; set; }
    }

    public class Node 
    {
        public RectangleF Rect { get; set; }
        public RectangleF ScaledRect { get; private set; }
        public bool HasMouseOver { get; private set; }
        public EventHandler OnClick { get; set; }
        
        public virtual void Update(IPlatformInfo ctx)
        {
            ScaledRect = new RectangleF(ctx.SizeH(Rect.X), ctx.SizeV(Rect.Y), ctx.SizeH(Rect.Width), ctx.SizeV(Rect.Height));
            HasMouseOver = ScaledRect.Contains(ctx.MousePosition);
            if(HasMouseOver && ctx.MouseClicked)
                OnClick?.Invoke(this, null);
        }
    }

    public class Box : Node
    {
        public Box(float x, float y, float w, float h, FillStyle fill = null, float cornerRadius = 0, Shadow shadow = null)
            : this(new RectangleF(x,y,w,h), fill, cornerRadius, shadow)
        {

        }
        public Box(RectangleF rect, FillStyle fill = null, float cornerRadius = 0, Shadow shadow = null)
        {
            Rect = rect;
            Fill = fill;
            CornerRadius = cornerRadius;
            Shadow = shadow;
        }

        public Box()
        {
        }

        public FillStyle Fill { get; set; }
        public FillStyle HoverFill { get; set; }
        public float CornerRadius { get; set; }
        public Shadow Shadow { get; set; }
        public float ScaledCorner { get; private set; }

        public override void Update(IPlatformInfo ctx)
        {
            base.Update(ctx);
            ScaledCorner = ctx.Size(CornerRadius);
        }


        public void Draw(NVGcontext vg)
        {
            
            // vg.BeginPath();
            // DrawShadow(ctx);
            // vg.Fill();

            var fill = Fill;
            if(HoverFill != null && HasMouseOver){
                fill = HoverFill;
            }     

            if(Fill != null){
                vg.BeginPath();
                fill.Apply(vg);
                DrawRect(vg);
                vg.Fill();
            }

        }

        private void DrawShadow(NVGcontext vg) 
        {
            // var vg = ctx.Canvas;
            // var inner = _scaledRect.Offset(Shadow.Size)
            // var paint = vg.BoxGradient(_scaledRect.X, _scaledRect.Y, _scaledRect.Width, _scaledRect,)

            // var shadowPaint = NanoVG.nvgBoxGradient(vg, x,y+1, w,h, cornerRadius+1, 6, NanoVG.nvgRGBA(0,0,0,45), NanoVG.nvgRGBA(0,0,0,0));
            // NanoVG.nvgBeginPath(vg);
            // NanoVG.nvgRect(vg, x-5,y-5, w+10,h+15);
            // NanoVG.nvgRoundedRect(vg, x,y, w,h, cornerRadius);
            // NanoVG.nvgPathWinding(vg, (int)NVGsolidity.NVG_HOLE);
            // NanoVG.nvgFillPaint(vg, shadowPaint);
            // NanoVG.nvgFill(vg);

            // NanoVG.nvgBeginPath(vg);
            // NanoVG.nvgRoundedRect(vg, x,y, w,h, cornerRadius);
            // NanoVG.nvgFillColor(vg, NanoVG.nvgRGBA(255,255,255,255));
            // NanoVG.nvgFill(vg);
        }

        private void DrawRect(NVGcontext vg)
        {
            if(CornerRadius > 0){
                vg.RoundedRect(ScaledRect.X, ScaledRect.Y, ScaledRect.Width, ScaledRect.Height, ScaledCorner);
            } else {
                vg.Rect(ScaledRect.X, ScaledRect.Y, ScaledRect.Width, ScaledRect.Height);
            }

        }


    }
}