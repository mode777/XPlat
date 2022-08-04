using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;
using XPlat.NanoVg;
using XPlat.WrenScripting;

namespace XPlat.Engine;

public static class IServiceCollectionExtensions
{
    public static void AddEngineServices(this IServiceCollection services){
        var config = services.BuildServiceProvider()
            .GetService<IConfiguration>()
            .GetSection("Engine")
            .Get<EngineConfiguration>();

        services.AddTransient<SceneReader>();

        var tr = new TypeRegistry();
        tr.LoadElementsFromAssembly(typeof(IServiceCollectionExtensions).Assembly);
        foreach (var item in config.Import)
        {
            tr.LoadElementsFromAssembly(item);
        }
        services.AddSingleton<TypeRegistry>(tr);

        foreach (var item in tr.SceneElements)
            if(!item.Value.IsAbstract && !item.Value.IsInterface)
                services.AddTransient(item.Value, item.Value);

        foreach (var item in tr.SceneTemplates)
            if(!item.Value.IsAbstract && !item.Value.IsInterface)
                services.AddTransient(item.Value, item.Value);

        foreach (var item in tr.Resources)
            if(!item.Value.IsAbstract && !item.Value.IsInterface)
                services.AddTransient(item.Value, item.Value);

        services.AddScoped<LuaHost>(s => {
            var l = new LuaHost();
            l.ImportNamespace(nameof(XPlat)+ "." + nameof(Core));
            return l;
        });
        services.AddScoped<WrenVm>(s => {
            var vm = new WrenVm();
            vm.Interpret("XPlat.Engine", File.ReadAllText("assets/wren/XPlat.Engine.wren"));
            vm.Interpret("XPlat.Core", File.ReadAllText("assets/wren/XPlat.Core.wren"));
            GC.Collect();
            GC.Collect();
            GC.Collect();
            return vm;
        });
        services.AddScoped<ResourceManager>();
        services.AddScoped<NVGcontext>(s => NVGcontext.CreateGl());

    }


}
