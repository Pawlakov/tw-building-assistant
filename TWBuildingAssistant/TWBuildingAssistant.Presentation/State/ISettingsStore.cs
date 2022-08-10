namespace TWBuildingAssistant.Presentation.State;

using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsStore
{
    Settings Settings { get; set; }
}