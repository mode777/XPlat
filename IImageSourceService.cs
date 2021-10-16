#nullable enable
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace Microsoft.Maui
{
	public interface IImageSourceService
	{
		Task<IImageSourceServiceResult<Image>?> GetImageAsync(
			IImageSource imageSource,
			float scale = 1,
			CancellationToken cancellationToken = default);
	}

	public interface IImageSourceService<in T> : IImageSourceService
		where T : IImageSource
	{
	}
}