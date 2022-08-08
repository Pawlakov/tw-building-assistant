namespace TWBuildingAssistant.Presentation.ViewModels;

using CommunityToolkit.Mvvm.Input;
using TWBuildingAssistant.Presentation.State;

public class MainWindowViewModel
    : WindowViewModel
{
    private readonly INavigator navigator;

    public MainWindowViewModel(INavigator navigator)
    {
        this.navigator = navigator;
        this.navigator.StateChanged += this.NavigatorStateChanged;
        this.navigator.CurrentViewType = INavigator.ViewType.Settings;
    }

    public ViewModel CurrentViewModel => this.navigator.CurrentViewModel;

    public RelayCommand<INavigator.ViewType> UpdateCurrentViewModelCommand { get; }

    public override void Dispose()
    {
        this.navigator.StateChanged -= this.NavigatorStateChanged;

        base.Dispose();
    }

    private void NavigatorStateChanged()
    {
        this.OnPropertyChanged(nameof(this.CurrentViewModel));
    }
}