namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;
using TWBuildingAssistant.Domain.StateModels;

public interface ISettingsStore
{
    Data.FSharp.Models.EffectSet Effect { get; set; }

    ImmutableArray<BuildingLibraryEntry> BuildingLibrary { get; set; }
}