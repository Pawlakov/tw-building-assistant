namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain.StateModels;

public class SettingsStore
    : ISettingsStore
{
    public Data.FSharp.Models.EffectSet Effect { get; set; }

    public ImmutableArray<BuildingLibraryEntry> BuildingLibrary { get; set; }
}