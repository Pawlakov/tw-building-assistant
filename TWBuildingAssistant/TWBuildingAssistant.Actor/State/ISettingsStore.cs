namespace TWBuildingAssistant.Actor.State;

using static TWBuildingAssistant.Domain.DTOs;

public interface ISettingsStore
{
    BuildingLibraryEntryDTO[] BuildingLibrary { get; set; }
}
