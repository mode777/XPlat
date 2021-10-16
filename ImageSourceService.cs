#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Microsoft.Maui
{
	public abstract class ImageSourceService : IImageSourceService
	{
		public ImageSourceService(ILogger? logger = null)
		{
			Logger = logger;
		}

		public ILogger? Logger { get; }

		public abstract Task<IImageSourceServiceResult<Image>?> GetImageAsync(
			IImageSource imageSource,
			float scale = 1,
			CancellationToken cancellationToken = default);

	}
}