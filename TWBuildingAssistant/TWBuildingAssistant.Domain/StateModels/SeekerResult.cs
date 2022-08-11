namespace TWBuildingAssistant.Domain.StateModels;

public readonly record struct SeekerResult(BuildingBranch Branch, BuildingLevel Level, int RegionId, int SlotIndex);