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
            // var transformed = typeof(NanoVG).GetMethods().Where(x => x.IsStatic && x.IsPublic).Select(x => new { 
            //     Name = x.Name.Replace("nvg", ""),
            //     Returns = x.ReturnType.Name,
            //     Args = x.GetParameters().Select(y => new {
            //         Type = y.ParameterType.Name,
            //         Name = y.Name,
            //         Combined = y.ParameterType.Name+" "+y.Name
            //     })
            // }).Select(x => $"public static {x.Returns} {x.Name}({(x.Args.First().Name == "ctx" ? "this " : String.Empty)}{String.Join(", ",x.Args.Select(y => y.Combined))}) === NanoVG.nvg{x.Name}({String.Join(", ", x.Args.Select(y => y.Name))});")
            // .Select(x => x.Replace("===", "=>").Replace("Single", "float").Replace("Int32", "int").Replace("Byte", "byte"))
            // .ToList();

            // var res = String.Join('\n', transformed);

            RunSdlApp<Pbr>(args, s => s.AddTransient<InkService>());
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


