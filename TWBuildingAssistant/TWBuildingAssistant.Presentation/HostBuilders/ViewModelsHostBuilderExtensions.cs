namespace TWBuildingAssistant.Presentation.HostBuilders;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBuildingAssistant.Presentation.ViewModels;
using TWBuildingAssistant.Presentation.ViewModels.Factories;

public static class ViewModelsHostBuilderExtensions
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

            services.AddSingleton<CreateViewModel<SettingsViewModel>>(services => () => services.GetRequiredService<SettingsViewModel>());
            services.AddSingleton<CreateViewModel<ProvinceViewModel>>(services => () => services.GetRequiredService<ProvinceViewModel>());
            services.AddSingleton<CreateViewModel<SeekerViewModel>>(services => () => services.GetRequiredService<SeekerViewModel>());

            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
        });

        return host;
    }
}