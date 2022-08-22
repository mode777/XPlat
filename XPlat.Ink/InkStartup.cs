using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XPlat.Engine;

namespace XPlat.Ink
{
    public class InkStartup : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WrenVmOptions>(x => {
                x.PreloadModules.Add("Ink.Runtime", File.ReadAllText("assets/wren/Ink.Runtime.wren"));
            });
        }
    }
}