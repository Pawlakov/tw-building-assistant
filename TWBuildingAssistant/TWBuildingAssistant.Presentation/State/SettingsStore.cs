namespace TWBuildingAssistant.Presentation.State;

using TWBuildingAssistant.Domain;
using static TWBuildingAssistant.Domain.Interface;

public class SettingsStore
    : ISettingsStore
{
    public BuildingLibraryEntryDto[] BuildingLibrary { get; set; }
}