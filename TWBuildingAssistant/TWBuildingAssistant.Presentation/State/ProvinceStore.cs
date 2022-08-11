namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using System.Collections.Immutable;
using TWBuildingAssistant.Domain.StateModels;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevelIds = new Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)>();
    }

    public Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevelIds { get; }

    public ImmutableArray<SeekerSettingsRegion> SeekerSettings { get; set; }
}