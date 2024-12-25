namespace TWBuildingAssistant.Actor.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Actor.ViewModels;
using TWBuildingAssistant.Actor.Views;

public static class ViewsHostBuilderExtensions
{
    public static IHostBuilder AddViews(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));
        });

        return host;
    }
}
