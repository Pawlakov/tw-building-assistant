namespace TWBuildingAssistant.Presentation.State;

using static TWBuildingAssistant.Domain.DTOs;

public class SettingsStore
    : ISettingsStore
{
    public BuildingLibraryEntryDto[] BuildingLibrary { get; set; }
}