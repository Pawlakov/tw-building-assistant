namespace TWBuildingAssistant.Presentation.State;

using System;
using TWBuildingAssistant.Presentation.ViewModels;
using TWBuildingAssistant.Presentation.ViewModels.Factories;

public class Navigator
    : INavigator
{
    private readonly IViewModelFactory viewModelFactory;

    private ViewModel currentViewModel;
    private INavigator.ViewType currentViewType;

    public Navigator(IViewModelFactory viewModelFactory)
    {
        this.viewModelFactory = viewModelFactory;
    }

    public event Action StateChanged;

    public ViewModel CurrentViewModel
    {
        get => this.currentViewModel;
    }

    public INavigator.ViewType CurrentViewType
    {
        get => this.currentViewType;
        set
        {
            this.currentViewModel?.Dispose();

            this.currentViewType = value;
            this.currentViewModel = this.viewModelFactory.CreateViewModel(this.currentViewType);
            this.StateChanged?.Invoke();
        }
    }
}
