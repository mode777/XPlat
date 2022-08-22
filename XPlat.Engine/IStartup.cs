using Microsoft.Extensions.DependencyInjection;

namespace XPlat.Engine;

public interface IStartup
{
    void ConfigureServices(IServiceCollection services);
}
