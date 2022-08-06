namespace TWBuildingAssistant.WorldManager.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.WorldManager.ViewModels;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<ViewModels.Resources.AddViewModel>();
            services.AddTransient<ViewModels.Resources.ListViewModel>();
        });

        return host;
    }
}