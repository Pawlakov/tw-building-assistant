namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using System.Collections.Immutable;
using TWBuildingAssistant.Domain.StateModels;

public interface IProvinceStore
{
    Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevelIds { get; }

    ImmutableArray<SeekerSettingsRegion> SeekerSettings { get; set; }

    List<SeekerResult> SeekerResults { get; }
}