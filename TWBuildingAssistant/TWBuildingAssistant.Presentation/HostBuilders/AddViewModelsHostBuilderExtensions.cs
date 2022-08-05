namespace TWBuildingAssistant.Presentation.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Presentation.ViewModels;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<ProvinceViewModel>();
            services.AddTransient<RegionViewModel>();
            services.AddTransient<SeekerViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SlotViewModel>();
        });

        return host;
    }
}