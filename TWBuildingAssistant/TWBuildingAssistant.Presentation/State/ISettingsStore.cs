namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsStore
{
    EffectSet Effect { get; set; }

    ImmutableArray<BuildingLibraryEntry> BuildingLibrary { get; set; }
}