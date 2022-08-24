namespace TWBuildingAssistant.Domain.StateModels;

public readonly record struct SeekerResult(Data.FSharp.Models.BuildingBranch Branch, Data.FSharp.Models.BuildingLevel Level, int RegionId, int SlotIndex);