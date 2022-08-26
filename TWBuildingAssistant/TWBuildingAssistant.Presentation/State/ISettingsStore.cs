namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Immutable;

public interface ISettingsStore
{
    Domain.Models.EffectSet Effect { get; set; }

    Domain.Models.BuildingLibraryEntry[] BuildingLibrary { get; set; }
}