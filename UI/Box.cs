using System.Drawing;
using NanoVGDotNet;

namespace net6test.UI
{

    public class Box : Node
    {
        public FillStyle? Fill { get; set; }
        public FillStyle? HoverFill { get; set; }
        public float CornerRadius { get; set; }
        public Shadow? Shadow { get; set; }
        public float ScaledCorner { get; private set; }
        public Thickness Padding { get; set; }
        internal Thickness ScaledPadding { get; private set; }
        public RectangleF PaddingBounds { get; private set; }

        public override void UpdateBounds(IPlatformInfo ctx)
        {
            base.UpdateBounds(ctx);
            ScaledCorner = ctx.Size(CornerRadius);
            Shadow?.Update(ctx);
            ScaledPadding = Padding.ToScaled(ctx);
            PaddingBounds = Bounds.Inset(ScaledPadding);
        }


        public void Draw(NVGcontext vg)
        {
            
            // vg.BeginPath();
            if(Shadow != null)
                DrawShadow(vg);
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
            var rlarge = Bounds.Expand(Shadow.SizeScaled);
            rlarge.Offset(Shadow.OffsetScaled);
            var trans = Shadow.Color;
            trans.a = 0;
            var paint = vg.BoxGradient(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, (Shadow.SizeScaled / 2f) + ScaledCorner, Shadow.SizeScaled, Shadow.Color, trans);
            vg.BeginPath();
            vg.Rect(rlarge.X, rlarge.Y, rlarge.Width, rlarge.Height);
            vg.PathWinding((int)NVGsolidity.NVG_HOLE);
            DrawRect(vg);
            vg.FillPaint(paint);
            vg.Fill();
        }

        private void DrawRect(NVGcontext vg)
        {
            if(CornerRadius > 0){
                vg.RoundedRect(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, ScaledCorner);
            } else {
                vg.Rect(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            }

        }


    }
}