namespace TWBuildingAssistant.Presentation.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TWBuildingAssistant.Model.Services;

public static class AddServicesHostBuilderExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder host)
    {
        return host.ConfigureServices((HostBuilderContext context, IServiceCollection collection) =>
        {
            collection.AddSingleton<IWorldDataService, WorldDataService>();
            collection.AddTransient<ISeekService, SeekService>();
        });
    }
}