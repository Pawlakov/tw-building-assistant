namespace TWBuildingAssistant.Presentation.State;

using TWBuildingAssistant.Domain;

public interface ISettingsStore
{
    Effects.EffectSet Effect { get; set; }

    Buildings.BuildingLibraryEntry[] BuildingLibrary { get; set; }
}