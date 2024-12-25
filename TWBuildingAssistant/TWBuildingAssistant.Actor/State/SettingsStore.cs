namespace TWBuildingAssistant.Actor.State;

using static TWBuildingAssistant.Domain.DTOs;

public class SettingsStore
    : ISettingsStore
{
    public BuildingLibraryEntryDTO[] BuildingLibrary { get; set; }
}
