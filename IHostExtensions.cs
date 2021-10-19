using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace net6test
{
    public static class IHostExtensions
    {
        public static void RunSdlApp(this IHost host, CancellationToken token = default)
        {
            try
            {
                var sdl = host.Services.GetRequiredService<SdlPlatform>();
                sdl.Init();
                host.StartAsync(token).GetAwaiter().GetResult();
                sdl.Run();
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