namespace Microsoft.Maui.Hosting
{
    public class FontManager : IFontManager
    {
        private IFontRegistrar fontRegistrar;
        private Extensions.Logging.ILogger<FontManager>? logger;

        public FontManager(IFontRegistrar fontRegistrar, Extensions.Logging.ILogger<FontManager>? logger)
        {
            this.fontRegistrar = fontRegistrar;
            this.logger = logger;
        }
    }
}