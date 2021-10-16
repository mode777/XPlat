#nullable enable
using Microsoft.Maui.Graphics;
using NanoVGDotNet;

namespace Microsoft.Maui
{
	public interface IFontImageSource : IImageSource
	{
		NVGcolor Color { get; }

		Font Font { get; }

		string Glyph { get; }
	}
}