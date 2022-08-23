namespace TWBuildingAssistant.Domain.StateModels;

public readonly record struct SeekerSettingsSlot(BuildingBranch? Branch, BuildingLevel? Level, Data.FSharp.Models.SlotDescriptor Descriptor, int RegionId, int SlotIndex);