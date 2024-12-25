namespace TWBuildingAssistant.Actor.ViewModels.Factories;

using TWBuildingAssistant.Actor.State;

public interface IViewModelFactory
{
    ViewModel CreateViewModel(INavigator.ViewType viewType);
}
