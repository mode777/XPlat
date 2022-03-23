using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace XPlat.Core
{
    public static class IHostExtensions
    {
        public static void RunSdlApp(this IHost host, CancellationToken token = default)
        {
            try
            {
                var sdl = host.Services.GetRequiredService<SdlPlatform>();
                sdl.Init();
                
                var app = host.Services.GetRequiredService<ISdlApp>();
                host.StartAsync(token).GetAwaiter().GetResult();
                sdl.Run(app);
                host.StopAsync(CancellationToken.None);
            }
            finally
            {
                if (host is IAsyncDisposable asyncDisposable)
                {
                    asyncDisposable.DisposeAsync().GetAwaiter().GetResult();
                }
                else
                {
                    host.Dispose();
                }
            }
        }
    }
}