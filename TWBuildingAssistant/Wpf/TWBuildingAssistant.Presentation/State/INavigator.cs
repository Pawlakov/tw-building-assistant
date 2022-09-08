namespace TWBuildingAssistant.Presentation.State;

using System;
using TWBuildingAssistant.Presentation.ViewModels;

public interface INavigator
{
    event Action StateChanged;

    public enum ViewType
    {
        Settings,
        Province,
        Seeker,
    }

    ViewModel CurrentViewModel { get; }

    ViewType CurrentViewType { get; set; }
}