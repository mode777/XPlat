using System;
using System.Collections.Generic;
using System.Numerics;
using XPlat.NanoVg;

namespace XPlat.NanoGui
{
    public class ImagePanel : Widget
    {
        public ImagePanel(Widget? parent) : base(parent)
        {
            this.ThumbSize = 64f;
            this.Spacing = 10f;
            this.Margin = 10f;
            this.MouseIndex = -1;
            Images = new List<int>();
        }

        public float ThumbSize { get; private set; }
        public float Spacing { get; private set; }
        public float Margin { get; private set; }
        public int MouseIndex { get; private set; }
        public List<int> Images { get; set; }
        public event EventHandler<int> OnImageClicked;

        Vector2 GridSize() {
            float nCols = 1 + MathF.Max(0, (Size.X - 2 * Margin - ThumbSize) / (ThumbSize + Spacing));
            float nRows = (Images.Count + nCols - 1) / nCols;
            return new Vector2(nCols, nRows);
        }

        int IndexForPosition(Vector2 p){
            var pp = (p-Position) - new Vector2(Margin, Margin) / (ThumbSize + Spacing);
            var iconRegion = ThumbSize / (ThumbSize + Spacing);
            bool overImage = pp.X - MathF.Floor(pp.X) < iconRegion &&
                             pp.Y - MathF.Floor(pp.Y) < iconRegion;
            var gridPos = pp;
            var grid = GridSize();
            if(overImage){
                overImage = gridPos.X >= 0 && gridPos.Y >= 0 && pp.X >= 0 && pp.Y >= 0 && gridPos.X < grid.X && gridPos.Y < grid.Y;
            }
            return overImage ? (int)(gridPos.X + gridPos.Y * grid.X) : -1;
        }

        public override bool MouseMotionEvent(Vector2 p, Vector2 rel, int button, int modifiers)
        {
            MouseIndex = IndexForPosition(p);
            return true;
        }

        public override bool MouseButtonEvent(Vector2 p, int button, bool down, int modifiers)
        {
            int index = IndexForPosition(p);
            if(index >= 0 && index < Images.Count && down) OnImageClicked?.Invoke(this, index);
            return true;
        }

        public override Vector2 PreferredSize(NVGcontext ctx)
        {
            var grid = GridSize();

            return new Vector2(
                grid.X * ThumbSize + (grid.X - 1) * Spacing + 2 * Margin,
                grid.Y * ThumbSize + (grid.Y - 1) * Spacing + 2 * Margin);
        }

        public override void Draw(NVGcontext vg)
        {
            var grid = GridSize();

            for (int i = 0; i < Images.Count; i++)
            {
                var p = Position + new Vector2(Margin, Margin) + new Vector2((int)(i % grid.X), (int)(i / grid.X)) * (ThumbSize + Spacing);
                int imgW = 0, imgH = 0;
                
                vg.ImageSize(Images[i], ref imgW, ref imgH);
                float iw = 0, ih = 0, ix = 0, iy = 0;
                if(imgW < imgH){
                    iw = ThumbSize;
                    ih = iw * (float)imgH / (float)imgW;
                    ix = 0;
                    iy = -(ih - ThumbSize) * 0.5f;
                } else {
                    ih = ThumbSize;
                    iw = ih * (float)imgW / (float)imgH;
                    ix = -(iw - ThumbSize) * 0.5f;
                    iy = 0;
                }

                var imgPaint = vg.ImagePattern(p.X + ix, p.Y + iy, iw, ih, 0, Images[i], MouseIndex == i ? 1 : 0.7f);

                vg.BeginPath();
                vg.RoundedRect(p.X, p.Y, ThumbSize, ThumbSize, 5);
                vg.FillPaint(imgPaint);
                vg.Fill();

                var shadowPaint = vg.BoxGradient(p.X - 1, p.Y, ThumbSize + 1, ThumbSize + 2, 5, 3, "#00000088", "#00000000");

                vg.BeginPath();
                vg.Rect(p.X - 5, p.Y-5, ThumbSize+10, ThumbSize+10);
                vg.RoundedRect(p.X, p.Y, ThumbSize, ThumbSize, 6);
                vg.PathWinding((int)NVGsolidity.NVG_HOLE);
                vg.FillPaint(shadowPaint);
                vg.Fill();

                vg.BeginPath();
                vg.RoundedRect(p.X+0.5f, p.Y+0.5f, ThumbSize-1, ThumbSize-1, 4-0.5f);
                vg.StrokeWidth(1.0f);
                vg.StrokeColor(vg.RGBA(255, 255, 255, 80));
                vg.Stroke();
            }
        }
    }
}