

namespace NanoVGDotNet
{
    public static class ContextExtensions
    {
        public static void StrokeColor(this NVGcontext ctx, NVGcolor color) => NanoVG.nvgStrokeColor(ctx, color);
        public static void StrokeWidth(this NVGcontext ctx, float width) => NanoVG.nvgStrokeWidth(ctx, width);
        public static void TransformScale(this NVGcontext ctx, float[] t, float sx, float sy) => NanoVG.nvgTransformScale(t, sx, sy);
        public static int TransformInverse(this NVGcontext ctx, float[] inv, float[] t) => NanoVG.nvgTransformInverse(inv, t);
        public static void Fill(this NVGcontext ctx) => NanoVG.nvgFill(ctx);
        public static void Rect(this NVGcontext ctx, float x, float y, float w, float h) => NanoVG.nvgRect(ctx, x, y, w, h);
        public static void Arc(this NVGcontext ctx, float cx, float cy, float r, float a0, float a1, int dir) => NanoVG.nvgArc(ctx, cx, cy, r, a0, a1, dir);
        public static void RoundedRect(this NVGcontext ctx, float x, float y, float w, float h, float r) => NanoVG.nvgRoundedRect(ctx, x, y, w, h, r);
        public static NVGpaint LinearGradient(this NVGcontext ctx, float sx, float sy, float ex, float ey, NVGcolor icol, NVGcolor ocol) => NanoVG.nvgLinearGradient(ctx, sx, sy, ex, ey, icol, ocol);
        public static NVGpaint RadialGradient(this NVGcontext ctx, float cx, float cy, float inr, float outr, NVGcolor icol, NVGcolor ocol) => NanoVG.nvgRadialGradient(ctx, cx, cy, inr, outr, icol, ocol);
        public static void Ellipse(this NVGcontext ctx, float cx, float cy, float rx, float ry) => NanoVG.nvgEllipse(ctx, cx, cy, rx, ry);
        public static void Circle(this NVGcontext ctx, float cx, float cy, float r) => NanoVG.nvgCircle(ctx, cx, cy, r);
        public static int TextGlyphPositions(this NVGcontext ctx, float x, float y, String string_, NVGglyphPosition[] positions, int maxPositions) => NanoVG.nvgTextGlyphPositions(ctx, x, y, string_, positions, maxPositions);
        public static void TextBox(this NVGcontext ctx, float x, float y, float breakRowWidth, String string_) => NanoVG.nvgTextBox(ctx, x, y, breakRowWidth, string_);
        public static void TextBoxBounds(this NVGcontext ctx, float x, float y, float breakRowWidth, String string_, float[] bounds) => NanoVG.nvgTextBoxBounds(ctx, x, y, breakRowWidth, string_, bounds);
        public static float TextBounds(this NVGcontext ctx, float x, float y, String string_, float[] bounds) => NanoVG.nvgTextBounds(ctx, x, y, string_, bounds);
        public static void TextMetrics(this NVGcontext ctx, ref float ascender, ref float descender, ref float lineh) => NanoVG.nvgTextMetrics(ctx, ref ascender, ref descender, ref lineh);
        public static int TextBreakLines(this NVGcontext ctx, String string_, float breakRowWidth, NVGtextRow[] rows, int maxRows) => NanoVG.nvgTextBreakLines(ctx, string_, breakRowWidth, rows, maxRows);
        public static void TextLineHeight(this NVGcontext ctx, float lineHeight) => NanoVG.nvgTextLineHeight(ctx, lineHeight);
        public static void TextAlign(this NVGcontext ctx, int align) => NanoVG.nvgTextAlign(ctx, align);
        public static void ImageSize(this NVGcontext ctx, int image, ref int w, ref int h) => NanoVG.nvgImageSize(ctx, image, ref w, ref h);
        public static NVGpaint ImagePattern(this NVGcontext ctx, float cx, float cy, float w, float h, float angle, int image, float alpha) => NanoVG.nvgImagePattern(ctx, cx, cy, w, h, angle, image, alpha);
        public static float Text(this NVGcontext ctx, float x, float y, String string_) => NanoVG.nvgText(ctx, x, y, string_);
        public static void GlobalAlpha(this NVGcontext ctx, float alpha) => NanoVG.nvgGlobalAlpha(ctx, alpha);
        public static NVGcolor HSLA(this NVGcontext ctx, float h, float s, float l, byte a) => NanoVG.nvgHSLA(h, s, l, a);
        public static int AddFallbackFontId(this NVGcontext ctx, int baseFont, int fallbackFont) => NanoVG.nvgAddFallbackFontId(ctx, baseFont, fallbackFont);
        public static void BeginFrame(this NVGcontext ctx, int windowWidth, int windowHeight, float devicePixelRatio) => NanoVG.nvgBeginFrame(ctx, windowWidth, windowHeight, devicePixelRatio);
        public static int CreateFont(this NVGcontext ctx, String internalFontName, String fileName) => NanoVG.nvgCreateFont(ctx, internalFontName, fileName);
        //public static byte[] ImageTobyteArray(Image imageIn) => NanoVG.nvgImageTobyteArray(imageIn);
        public static int CreateImage(this NVGcontext ctx, String filename, int imageFlags) => NanoVG.nvgCreateImage(ctx, filename, imageFlags);
        public static void FontSize(this NVGcontext ctx, float size) => NanoVG.nvgFontSize(ctx, size);
        public static void FontBlur(this NVGcontext ctx, float blur) => NanoVG.nvgFontBlur(ctx, blur);
        public static void FontFace(this NVGcontext ctx, String font) => NanoVG.nvgFontFace(ctx, font);
        public static NVGcolor RGBA(this NVGcontext ctx, byte r, byte g, byte b, byte a) => NanoVG.nvgRGBA(r, g, b, a);
        public static NVGpaint BoxGradient(this NVGcontext ctx, float x, float y, float w, float h, float r, float f, NVGcolor icol, NVGcolor ocol) => NanoVG.nvgBoxGradient(ctx, x, y, w, h, r, f, icol, ocol);
        public static void ClosePath(this NVGcontext ctx) => NanoVG.nvgClosePath(ctx);
        public static void PathWinding(this NVGcontext ctx, int dir) => NanoVG.nvgPathWinding(ctx, dir);
        public static void Stroke(this NVGcontext ctx) => NanoVG.nvgStroke(ctx);
        public static void Save(this NVGcontext ctx) => NanoVG.nvgSave(ctx);
        public static void Restore(this NVGcontext ctx) => NanoVG.nvgRestore(ctx);
        public static float DegToRad(this NVGcontext ctx, float deg) => NanoVG.nvgDegToRad(deg);
        public static void Scissor(this NVGcontext ctx, float x, float y, float w, float h) => NanoVG.nvgScissor(ctx, x, y, w, h);
        public static void IntersectScissor(this NVGcontext ctx, float x, float y, float w, float h) => NanoVG.nvgIntersectScissor(ctx, x, y, w, h);
        public static void ResetScissor(this NVGcontext ctx) => NanoVG.nvgResetScissor(ctx);
        public static void Rotate(this NVGcontext ctx, float angle) => NanoVG.nvgRotate(ctx, angle);
        public static void Scale(this NVGcontext ctx, float x, float y) => NanoVG.nvgScale(ctx, x, y);
        public static void Translate(this NVGcontext ctx, float x, float y) => NanoVG.nvgTranslate(ctx, x, y);
        public static void DeleteImage(this NVGcontext ctx, int image) => NanoVG.nvgDeleteImage(ctx, image);
        public static void EndFrame(this NVGcontext ctx) => NanoVG.nvgEndFrame(ctx);
        public static void BeginPath(this NVGcontext ctx) => NanoVG.nvgBeginPath(ctx);
        public static void TransformMultiply(float[] t, float[] s) => NanoVG.nvgTransformMultiply(t, s);
        public static void LineJoin(this NVGcontext ctx, int join) => NanoVG.nvgLineJoin(ctx, join);
        public static void MoveTo(this NVGcontext ctx, float x, float y) => NanoVG.nvgMoveTo(ctx, x, y);
        public static void BezierTo(this NVGcontext ctx, float c1x, float c1y, float c2x, float c2y, float x, float y) => NanoVG.nvgBezierTo(ctx, c1x, c1y, c2x, c2y, x, y);
        public static void LineTo(this NVGcontext ctx, float x, float y) => NanoVG.nvgLineTo(ctx, x, y);
        public static void LineCap(this NVGcontext ctx, int cap) => NanoVG.nvgLineCap(ctx, cap);
        public static void FillPaint(this NVGcontext ctx, NVGpaint paint) => NanoVG.nvgFillPaint(ctx, paint);
        public static void FillColor(this NVGcontext ctx, NVGcolor color) => NanoVG.nvgFillColor(ctx, color);
    }
}