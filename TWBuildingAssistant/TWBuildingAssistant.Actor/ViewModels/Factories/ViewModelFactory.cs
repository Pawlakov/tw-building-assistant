namespace TWBuildingAssistant.Actor.ViewModels.Factories;

using System;
using TWBuildingAssistant.Actor.State;

public delegate TViewModel CreateViewModel<TViewModel>()
    where TViewModel : ViewModel;

public class ViewModelFactory
    : IViewModelFactory
{
    private readonly CreateViewModel<SettingsViewModel> createSettingsViewModel;
    private readonly CreateViewModel<ProvinceViewModel> createProvinceViewModel;

    public ViewModelFactory(
        CreateViewModel<SettingsViewModel> createSettingsViewModel,
        CreateViewModel<ProvinceViewModel> createProvinceViewModel)
    {
        this.createSettingsViewModel = createSettingsViewModel;
        this.createProvinceViewModel = createProvinceViewModel;
    }

    public ViewModel CreateViewModel(INavigator.ViewType viewType)
    {
        return viewType switch
        {
            INavigator.ViewType.Settings =>
                this.createSettingsViewModel(),
            INavigator.ViewType.Province =>
                this.createProvinceViewModel(),
            _ =>
                throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
        };
    }
}