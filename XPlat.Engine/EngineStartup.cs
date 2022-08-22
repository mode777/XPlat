using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;
using XPlat.NanoVg;
using XPlat.WrenScripting;

namespace XPlat.Engine;

public class EngineStartup : IStartup
{
    private readonly EngineConfiguration config;

    public EngineStartup(EngineConfiguration config)
    {
        this.config = config;
    }

    public void ConfigureServices(IServiceCollection services)
    {
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
        services.AddScoped<WrenVm>(CreateWrenVm);
        services.AddScoped<ResourceManager>();
        services.AddScoped<NVGcontext>(s => NVGcontext.CreateGl());

        services.Configure<WrenVmOptions>(options => {
            options.PreloadModules.Add("XPlat.Engine", File.ReadAllText("assets/wren/XPlat.Engine.wren"));
            options.PreloadModules.Add("XPlat.Engine.Components", File.ReadAllText("assets/wren/XPlat.Engine.Components.wren"));
            options.PreloadModules.Add("XPlat.Core", File.ReadAllText("assets/wren/XPlat.Core.wren"));
            options.PreloadModules.Add("XPlat.NanoVg", File.ReadAllText("assets/wren/XPlat.NanoVg.wren"));
        });
    }

    private WrenVm CreateWrenVm(IServiceProvider services){
        var vm = new WrenVm();
        var options = services.GetRequiredService<IOptions<WrenVmOptions>>();
        foreach (var kv in options.Value.PreloadModules)
        {
            vm.Interpret(kv.Key, kv.Value);
        }

        //var c = vm.GetClass("XPlat.NanoVg", "NVGcontext");
        //c.RegisterCustomBinding("fontColor", new NVGcontext_FillColor(vm, c));
        return vm;
    }
}
