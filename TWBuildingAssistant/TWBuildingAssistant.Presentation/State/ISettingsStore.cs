namespace TWBuildingAssistant.Presentation.State;

using TWBuildingAssistant.Domain;
using static TWBuildingAssistant.Domain.Interface;

public interface ISettingsStore
{
    BuildingLibraryEntryDto[] BuildingLibrary { get; set; }
}