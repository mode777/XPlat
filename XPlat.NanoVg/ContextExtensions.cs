

using System;

namespace XPlat.NanoVg
{
    public static class ContextExtensions
    {
        public static void StrokeColor(this NVGcontext ctx, NVGcolor color) => NanoVgApi.nvgStrokeColor(ctx, color);
        public static void StrokeWidth(this NVGcontext ctx, float width) => NanoVgApi.nvgStrokeWidth(ctx, width);
        public static void TransformScale(this NVGcontext ctx, float[] t, float sx, float sy) => NanoVgApi.nvgTransformScale(t, sx, sy);
        public static int TransformInverse(this NVGcontext ctx, float[] inv, float[] t) => NanoVgApi.nvgTransformInverse(inv, t);
        public static void Fill(this NVGcontext ctx) => NanoVgApi.nvgFill(ctx);
        public static void Rect(this NVGcontext ctx, float x, float y, float w, float h) => NanoVgApi.nvgRect(ctx, x, y, w, h);
        public static void Arc(this NVGcontext ctx, float cx, float cy, float r, float a0, float a1, int dir) => NanoVgApi.nvgArc(ctx, cx, cy, r, a0, a1, dir);
        public static void RoundedRect(this NVGcontext ctx, float x, float y, float w, float h, float r) => NanoVgApi.nvgRoundedRect(ctx, x, y, w, h, r);
        public static NVGpaint LinearGradient(this NVGcontext ctx, float sx, float sy, float ex, float ey, NVGcolor icol, NVGcolor ocol) => NanoVgApi.nvgLinearGradient(ctx, sx, sy, ex, ey, icol, ocol);
        public static NVGpaint RadialGradient(this NVGcontext ctx, float cx, float cy, float inr, float outr, NVGcolor icol, NVGcolor ocol) => NanoVgApi.nvgRadialGradient(ctx, cx, cy, inr, outr, icol, ocol);
        public static void Ellipse(this NVGcontext ctx, float cx, float cy, float rx, float ry) => NanoVgApi.nvgEllipse(ctx, cx, cy, rx, ry);
        public static void Circle(this NVGcontext ctx, float cx, float cy, float r) => NanoVgApi.nvgCircle(ctx, cx, cy, r);
        public static int TextGlyphPositions(this NVGcontext ctx, float x, float y, String string_, NVGglyphPosition[] positions, int maxPositions) => NanoVgApi.nvgTextGlyphPositions(ctx, x, y, string_, positions, maxPositions);
        public static void TextBox(this NVGcontext ctx, float x, float y, float breakRowWidth, String string_) => NanoVgApi.nvgTextBox(ctx, x, y, breakRowWidth, string_);
        public static void TextBoxBounds(this NVGcontext ctx, float x, float y, float breakRowWidth, String string_, float[] bounds) => NanoVgApi.nvgTextBoxBounds(ctx, x, y, breakRowWidth, string_, bounds);
        public static float TextBounds(this NVGcontext ctx, float x, float y, String string_, float[] bounds) => NanoVgApi.nvgTextBounds(ctx, x, y, string_, bounds);
        public static void TextMetrics(this NVGcontext ctx, ref float ascender, ref float descender, ref float lineh) => NanoVgApi.nvgTextMetrics(ctx, ref ascender, ref descender, ref lineh);
        public static int TextBreakLines(this NVGcontext ctx, String string_, float breakRowWidth, NVGtextRow[] rows, int maxRows) => NanoVgApi.nvgTextBreakLines(ctx, string_, breakRowWidth, rows, maxRows);
        public static void TextLineHeight(this NVGcontext ctx, float lineHeight) => NanoVgApi.nvgTextLineHeight(ctx, lineHeight);
        public static void TextAlign(this NVGcontext ctx, int align) => NanoVgApi.nvgTextAlign(ctx, align);
        public static void ImageSize(this NVGcontext ctx, int image, ref int w, ref int h) => NanoVgApi.nvgImageSize(ctx, image, ref w, ref h);
        public static NVGpaint ImagePattern(this NVGcontext ctx, float cx, float cy, float w, float h, float angle, int image, float alpha) => NanoVgApi.nvgImagePattern(ctx, cx, cy, w, h, angle, image, alpha);
        public static float Text(this NVGcontext ctx, float x, float y, String string_) => NanoVgApi.nvgText(ctx, x, y, string_);
        public static void GlobalAlpha(this NVGcontext ctx, float alpha) => NanoVgApi.nvgGlobalAlpha(ctx, alpha);
        public static NVGcolor HSLA(this NVGcontext ctx, float h, float s, float l, byte a) => NanoVgApi.nvgHSLA(h, s, l, a);
        public static int AddFallbackFontId(this NVGcontext ctx, int baseFont, int fallbackFont) => NanoVgApi.nvgAddFallbackFontId(ctx, baseFont, fallbackFont);
        public static void BeginFrame(this NVGcontext ctx, int windowWidth, int windowHeight, float devicePixelRatio) => NanoVgApi.nvgBeginFrame(ctx, windowWidth, windowHeight, devicePixelRatio);
        public static int CreateFont(this NVGcontext ctx, String internalFontName, String fileName) => NanoVgApi.nvgCreateFont(ctx, internalFontName, fileName);
        //public static byte[] ImageTobyteArray(Image imageIn) => NanoVG.nvgImageTobyteArray(imageIn);
        public static int CreateImage(this NVGcontext ctx, String filename, int imageFlags) => NanoVgApi.nvgCreateImage(ctx, filename, imageFlags);
        public static void FontSize(this NVGcontext ctx, float size) => NanoVgApi.nvgFontSize(ctx, size);
        public static void FontBlur(this NVGcontext ctx, float blur) => NanoVgApi.nvgFontBlur(ctx, blur);
        public static void FontFace(this NVGcontext ctx, String font) => NanoVgApi.nvgFontFace(ctx, font);
        public static NVGcolor RGBA(this NVGcontext ctx, byte r, byte g, byte b, byte a) => NanoVgApi.nvgRGBA(r, g, b, a);
        public static NVGpaint BoxGradient(this NVGcontext ctx, float x, float y, float w, float h, float r, float f, NVGcolor icol, NVGcolor ocol) => NanoVgApi.nvgBoxGradient(ctx, x, y, w, h, r, f, icol, ocol);
        public static void ClosePath(this NVGcontext ctx) => NanoVgApi.nvgClosePath(ctx);
        public static void PathWinding(this NVGcontext ctx, int dir) => NanoVgApi.nvgPathWinding(ctx, dir);
        public static void Stroke(this NVGcontext ctx) => NanoVgApi.nvgStroke(ctx);
        public static void Save(this NVGcontext ctx) => NanoVgApi.nvgSave(ctx);
        public static void Restore(this NVGcontext ctx) => NanoVgApi.nvgRestore(ctx);
        public static float DegToRad(this NVGcontext ctx, float deg) => NanoVgApi.nvgDegToRad(deg);
        public static void Scissor(this NVGcontext ctx, float x, float y, float w, float h) => NanoVgApi.nvgScissor(ctx, x, y, w, h);
        public static void IntersectScissor(this NVGcontext ctx, float x, float y, float w, float h) => NanoVgApi.nvgIntersectScissor(ctx, x, y, w, h);
        public static void ResetScissor(this NVGcontext ctx) => NanoVgApi.nvgResetScissor(ctx);
        public static void Rotate(this NVGcontext ctx, float angle) => NanoVgApi.nvgRotate(ctx, angle);
        public static void Scale(this NVGcontext ctx, float x, float y) => NanoVgApi.nvgScale(ctx, x, y);
        public static void Translate(this NVGcontext ctx, float x, float y) => NanoVgApi.nvgTranslate(ctx, x, y);
        public static void DeleteImage(this NVGcontext ctx, int image) => NanoVgApi.nvgDeleteImage(ctx, image);
        public static void EndFrame(this NVGcontext ctx) => NanoVgApi.nvgEndFrame(ctx);
        public static void BeginPath(this NVGcontext ctx) => NanoVgApi.nvgBeginPath(ctx);
        public static void TransformMultiply(float[] t, float[] s) => NanoVgApi.nvgTransformMultiply(t, s);
        public static void LineJoin(this NVGcontext ctx, int join) => NanoVgApi.nvgLineJoin(ctx, join);
        public static void MoveTo(this NVGcontext ctx, float x, float y) => NanoVgApi.nvgMoveTo(ctx, x, y);
        public static void BezierTo(this NVGcontext ctx, float c1x, float c1y, float c2x, float c2y, float x, float y) => NanoVgApi.nvgBezierTo(ctx, c1x, c1y, c2x, c2y, x, y);
        public static void QuadTo(this NVGcontext ctx, float cx, float cy, float x, float y) => NanoVgApi.nvgQuadTo(ctx, cx, cy, x, y);
        public static void ArcTo(NVGcontext ctx, float x1, float y1, float x2, float y2, float radius) => NanoVgApi.nvgArcTo(ctx, x1, y1, x2, y2, radius);
        public static void LineTo(this NVGcontext ctx, float x, float y) => NanoVgApi.nvgLineTo(ctx, x, y);
        public static void LineCap(this NVGcontext ctx, int cap) => NanoVgApi.nvgLineCap(ctx, cap);
        public static void FillPaint(this NVGcontext ctx, NVGpaint paint) => NanoVgApi.nvgFillPaint(ctx, paint);
        public static void FillColor(this NVGcontext ctx, NVGcolor color) => NanoVgApi.nvgFillColor(ctx, color);
    }
}