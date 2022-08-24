namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;

public interface ISettingsStore
{
    Data.FSharp.Models.EffectSet Effect { get; set; }

    ImmutableArray<Data.FSharp.Models.BuildingLibraryEntry> BuildingLibrary { get; set; }
}