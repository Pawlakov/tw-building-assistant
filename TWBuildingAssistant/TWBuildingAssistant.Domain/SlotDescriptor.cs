namespace TWBuildingAssistant.Domain;

using TWBuildingAssistant.Data.Model;

public readonly record struct SlotDescriptor(SlotType SlotType, RegionType RegionType, int? ResourceId);
