namespace TWBuildingAssistant.Presentation.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Presentation.State;

public static class PresentationServicesHostBuilderExtensions
{
    public static IHostBuilder AddStores(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<IWorldStore, WorldStore>();
            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<ISettingsStore, SettingsStore>();
        });

        return host;
    }
}