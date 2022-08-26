namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;

public class SettingsStore
    : ISettingsStore
{
    public Domain.Models.EffectSet Effect { get; set; }

    public Domain.Models.BuildingLibraryEntry[] BuildingLibrary { get; set; }
}