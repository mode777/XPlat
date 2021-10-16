#nullable enable
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Microsoft.Maui
{
	public class StreamImageSourceService : ImageSourceService, IImageSourceService<IStreamImageSource>
	{
		public StreamImageSourceService()
			: this(null)
		{
		}

		public StreamImageSourceService(ILogger<StreamImageSourceService>? logger = null)
			: base(logger)
		{
		}

        public override Task<IImageSourceServiceResult<Image>?> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}