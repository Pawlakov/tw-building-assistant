namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using TWBuildingAssistant.Domain;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevelIds = new Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)>();
    }

    public Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevelIds { get; }
}