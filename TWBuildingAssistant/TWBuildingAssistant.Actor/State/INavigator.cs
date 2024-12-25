namespace TWBuildingAssistant.Actor.State;

using System;
using TWBuildingAssistant.Actor.ViewModels;

public interface INavigator
{
    event Action StateChanged;

    public enum ViewType
    {
        Settings,
        Province,
    }

    ViewModel CurrentViewModel { get; }

    ViewType CurrentViewType { get; set; }
}
