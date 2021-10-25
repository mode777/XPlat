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

namespace net_gles2
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            RunSdlApp<Lightmap>(args, s => s.AddTransient<InkService>());
        }

        public static void RunSdlApp<T>(string[] args, Action<IServiceCollection>? addServices = null) where T : class, ISdlApp =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<SdlPlatform>();
                    services.AddSingleton<ISdlApp, T>();
                    var pi = new PlatformInfo();
                    IPlatformInfo.Default = pi;
                    services.AddSingleton<IPlatformInfo>(pi);
                    services.AddSingleton<PlatformInfo>(pi);
                    addServices?.Invoke(services);
                    
                }).Build().RunSdlApp();
    }
}


