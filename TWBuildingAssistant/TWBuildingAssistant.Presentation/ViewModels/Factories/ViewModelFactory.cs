namespace TWBuildingAssistant.Presentation.ViewModels.Factories;

using System;
using TWBuildingAssistant.Presentation.State;

public delegate TViewModel CreateViewModel<TViewModel>() 
    where TViewModel : ViewModel;

public class ViewModelFactory
    : IViewModelFactory
{
    private readonly CreateViewModel<SettingsViewModel> createSettingsViewModel;
    private readonly CreateViewModel<ProvinceViewModel> createProvinceViewModel;
    private readonly CreateViewModel<SeekerViewModel> createSeekerViewModel;

    public ViewModelFactory(
        CreateViewModel<SettingsViewModel> createSettingsViewModel,
        CreateViewModel<ProvinceViewModel> createProvinceViewModel,
        CreateViewModel<SeekerViewModel> createSeekerViewModel)
    {
        this.createSettingsViewModel = createSettingsViewModel;
        this.createProvinceViewModel = createProvinceViewModel;
        this.createSeekerViewModel = createSeekerViewModel;
    }

    public ViewModel CreateViewModel(INavigator.ViewType viewType)
    {
        return viewType switch
        {
            INavigator.ViewType.Settings =>
                this.createSettingsViewModel(),
            INavigator.ViewType.Province =>
                this.createProvinceViewModel(),
            INavigator.ViewType.Seeker =>
                this.createSeekerViewModel(),
            _ =>
                throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType"),
        };
    }
}