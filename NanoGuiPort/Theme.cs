using NanoVGDotNet;

namespace net6test.NanoGuiPort
{
    public class Theme
    {
        private NVGcontext nvgContext;

        public Theme(NVGcontext nvgContext)
        {
            this.nvgContext = nvgContext;
        }

        public float IconScale { get; internal set; }
        public float StandardFontSize { get; internal set; }
    }
}