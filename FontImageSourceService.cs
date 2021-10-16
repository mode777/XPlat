#nullable enable
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Microsoft.Maui
{
	public class FontImageSourceService : ImageSourceService, IImageSourceService<IFontImageSource>
	{
		public FontImageSourceService(IFontManager fontManager)
			: this(fontManager, null)
		{
		}

		public FontImageSourceService(IFontManager fontManager, ILogger<FontImageSourceService>? logger = null)
			: base(logger)
		{
			FontManager = fontManager;
		}

		public IFontManager FontManager { get; }

        public override Task<IImageSourceServiceResult<Image>?> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}