#nullable enable
using System.IO;
using Microsoft.Extensions.Logging;

namespace Microsoft.Maui
{
	public class EmbeddedFontLoader : IEmbeddedFontLoader
	{
		readonly ILogger<EmbeddedFontLoader>? _logger;

		public EmbeddedFontLoader()
			: this(null)
		{
		}

		public EmbeddedFontLoader(ILogger<EmbeddedFontLoader>? logger = null)
		{
			_logger = logger;
		}

        public string? LoadFont(EmbeddedFont font)
        {
            throw new NotImplementedException();
        }
    }
}