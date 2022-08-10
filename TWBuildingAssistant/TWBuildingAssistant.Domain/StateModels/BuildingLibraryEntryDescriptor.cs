namespace TWBuildingAssistant.Domain.StateModels;

using TWBuildingAssistant.Data.Model;

public readonly record struct BuildingLibraryEntryDescriptor(SlotType SlotType, RegionType RegionType, int? ResourceId);
