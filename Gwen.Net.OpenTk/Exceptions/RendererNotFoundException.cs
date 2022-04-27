using System;

namespace Gwen.Net.OpenTk.Exceptions
{
    public class RendererNotFoundException : Exception
    {
        public GwenGuiRenderer Renderer { get; }

        public RendererNotFoundException(GwenGuiRenderer renderer)
            : base(string.Format(StringResources.RenderNotFoundFormat, Enum.GetName(typeof(GwenGuiRenderer), renderer)))
        {
            Renderer = renderer;
        }
    }
}
