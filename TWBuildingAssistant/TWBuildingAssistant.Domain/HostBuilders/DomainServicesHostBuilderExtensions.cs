namespace TWBuildingAssistant.Domain.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Domain.Services;

public static class DomainServicesHostBuilderExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder host)
    {
        return host.ConfigureServices((HostBuilderContext context, IServiceCollection collection) =>
        {
            collection.AddTransient<ISeekService, SeekService>();
            collection.AddTransient<IProvinceService, ProvinceService>();
        });
    }
}