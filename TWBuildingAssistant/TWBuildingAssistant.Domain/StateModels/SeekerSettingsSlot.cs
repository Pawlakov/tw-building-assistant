namespace TWBuildingAssistant.Domain.StateModels;

public readonly record struct SeekerSettingsSlot(Data.FSharp.Models.BuildingBranch? Branch, Data.FSharp.Models.BuildingLevel? Level, Data.FSharp.Models.SlotDescriptor Descriptor, int RegionId, int SlotIndex);