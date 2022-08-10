namespace TWBuildingAssistant.Presentation.State;

using TWBuildingAssistant.Domain.StateModels;

public class SettingsStore
    : ISettingsStore
{
    public Settings Settings { get; set; }
}