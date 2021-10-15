using System;

namespace Microsoft.Maui
{
	public interface IMauiContext
	{
		IServiceProvider Services { get; }

		IMauiHandlersServiceProvider Handlers { get; }
	}
}