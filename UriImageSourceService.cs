#nullable enable
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Microsoft.Maui
{
	public class UriImageSourceService : ImageSourceService, IImageSourceService<IUriImageSource>
	{
		public UriImageSourceService()
			: this(null)
		{
		}

		public UriImageSourceService(ILogger<UriImageSourceService>? logger = null)
			: base(logger)
		{
		}

        public override Task<IImageSourceServiceResult<Image>?> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}