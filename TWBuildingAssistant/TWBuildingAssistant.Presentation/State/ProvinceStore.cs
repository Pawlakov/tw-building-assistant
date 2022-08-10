namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using TWBuildingAssistant.Domain.OldModels;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevelIds = new Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)>();
    }

    public List<BuildingSlot> OldStyleSlots { get; set; }

    public Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevelIds { get; }
}