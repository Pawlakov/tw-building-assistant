﻿namespace TWBuildingAssistant.Domain.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Domain.State;

public static class AddStoresHostBuilderExtensions
{
    public static IHostBuilder AddStores(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<IWorldStore, WorldStore>();
        });

        return host;
    }
}