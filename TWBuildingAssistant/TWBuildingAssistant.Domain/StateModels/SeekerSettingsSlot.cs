namespace TWBuildingAssistant.Domain.StateModels;

public readonly record struct SeekerSettingsSlot(BuildingBranch? Branch, BuildingLevel? Level, SlotDescriptor Descriptor, int RegionId, int SlotIndex);