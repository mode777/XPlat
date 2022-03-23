using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace XPlat.Core
{
	public static class XPlatApp
	{
        public static void RunSdl<T>(string[] args, Action<IServiceCollection>? addServices = null) where T : class, ISdlApp =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<SdlPlatform>();
                services.AddSingleton<ISdlApp, T>();
                addServices?.Invoke(services);

                services.AddSingleton<ISdlPlatformEvents>(x => x.GetRequiredService<SdlPlatform>());
                services.AddSingleton<IPlatform>(x => x.GetRequiredService<SdlPlatform>());

            }).Build().RunSdlApp();
    }
}

