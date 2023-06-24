namespace TWBuildingAssistant.Presentation.State;

using static TWBuildingAssistant.Domain.DTOs;

public interface ISettingsStore
{
    BuildingLibraryEntryDTO[] BuildingLibrary { get; set; }
}