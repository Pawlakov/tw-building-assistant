namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsStore
{
    Settings Settings { get; set; }

    EffectSet Effect { get; set; }

    ImmutableArray<BuildingLibraryEntry> BuildingLibrary { get; set; }
}