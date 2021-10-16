using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui.Animations;

namespace Microsoft.Maui.Hosting
{
	public static partial class AppHostBuilderExtensions
	{
		public static MauiAppBuilder ConfigureAnimations(this MauiAppBuilder builder)
		{
			builder.Services.TryAddTransient<ITicker>(svcs => new NativeTicker());
			builder.Services.TryAddTransient<IAnimationManager>(svcs => new AnimationManager(svcs.GetRequiredService<ITicker>()));
			return builder;
		}
	}
}