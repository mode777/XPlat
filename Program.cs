using System;
using SDL2;
using GLES2;
using net6test;
using net6test.samples;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using NanoVGDotNet;
using net6test.WorldGenerator;

namespace net_gles2
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            RunSdlApp<NanoGuiPortDemo>(args, s => s.AddTransient<InkService>());
        }

        public static void RunSdlApp<T>(string[] args, Action<IServiceCollection>? addServices = null) where T : class, ISdlApp =>
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


