#nullable enable
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Microsoft.Maui
{
	public class FileImageSourceService : ImageSourceService, IImageSourceService<IFileImageSource>
	{
		public FileImageSourceService()
			: this(null, null)
		{
		}

		public FileImageSourceService(IImageSourceServiceConfiguration? configuration = null, ILogger<FileImageSourceService>? logger = null)
			: base(logger)
		{
			Configuration = configuration;
		}

		public IImageSourceServiceConfiguration? Configuration { get; }

        public override Task<IImageSourceServiceResult<Image>?> GetImageAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}