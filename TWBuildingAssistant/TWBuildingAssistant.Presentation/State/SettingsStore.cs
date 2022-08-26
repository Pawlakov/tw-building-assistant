namespace TWBuildingAssistant.Presentation.State;

using TWBuildingAssistant.Domain;

public class SettingsStore
    : ISettingsStore
{
    public Effects.EffectSet Effect { get; set; }

    public Buildings.BuildingLibraryEntry[] BuildingLibrary { get; set; }
}