namespace TWBuildingAssistant.Presentation.State;

using static TWBuildingAssistant.Domain.DTOs;

public interface ISettingsStore
{
    BuildingLibraryEntryDto[] BuildingLibrary { get; set; }
}