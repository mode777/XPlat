using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;
using XPlat.NanoVg;
using XPlat.WrenScripting;

namespace XPlat.Engine;

// public class NVGcontext_FillColor : WrenForeignMethod
// {
//     public NVGcontext_FillColor(WrenVm vm, WrenForeignClass owner) : base(vm, owner, false, typeof(NVGcontext).GetMethods().FirstOrDefault(x => x.Name == "FillColor"))
//     {
//     }

//     public override void Invoke(IntPtr vm)
//     {
//         var target = GetTarget(vm);
//         var str = (string)GetWrenSlot(vm, typeof(string), 1);
//         Parameters[0] = (NVGcolor)str;
//         MethodInfo.Invoke(target, Parameters);
//     }
// }

public static class WrenVmExtensions {
    public static void AddEngineModules(this WrenVm vm){
        vm.Interpret("XPlat.Engine", File.ReadAllText("assets/wren/XPlat.Engine.wren"));
        vm.Interpret("XPlat.Engine.Components", File.ReadAllText("assets/wren/XPlat.Engine.Components.wren"));
        vm.Interpret("XPlat.Core", File.ReadAllText("assets/wren/XPlat.Core.wren"));
        vm.Interpret("XPlat.NanoVg", File.ReadAllText("assets/wren/XPlat.NanoVg.wren"));
        //var c = vm.GetClass("XPlat.NanoVg", "NVGcontext");
        //c.RegisterCustomBinding("fontColor", new NVGcontext_FillColor(vm, c));
    }
}

public static class IServiceCollectionExtensions
{
    public static void AddEngineServices(this IServiceCollection services){
        var configProvider = services.BuildServiceProvider();
        var config = configProvider
            .GetService<IConfiguration>()
            .GetSection("Engine")
            .Get<EngineConfiguration>();

        services.AddSingleton<EngineConfiguration>(config);

        var startups = AssemblyScanner.FindTypes<IStartup>(typeof(IServiceCollectionExtensions).Assembly)
            .Concat(config.Import
                .SelectMany(x => AssemblyScanner.FindTypes<IStartup>(AssemblyScanner.LoadAssembly(x))))
            .ToArray();

        foreach (var item in startups)
        {
            services.AddTransient(item, item);
        }
        var startupProvider = services.BuildServiceProvider();
        foreach (var item in startups)
        {
            var startup = startupProvider.GetRequiredService(item) as IStartup;
            startup.ConfigureServices(services);
        }
    }
}
