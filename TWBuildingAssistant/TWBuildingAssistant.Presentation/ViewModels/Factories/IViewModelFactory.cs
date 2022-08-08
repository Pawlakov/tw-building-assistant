namespace TWBuildingAssistant.Presentation.ViewModels.Factories;

using TWBuildingAssistant.Presentation.State;

public interface IViewModelFactory
{
    ViewModel CreateViewModel(INavigator.ViewType viewType);
}