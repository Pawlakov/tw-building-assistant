namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using TWBuildingAssistant.Domain.OldModels;

public interface IProvinceStore
{
    List<BuildingSlot> OldStyleSlots { get; set; }

    Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevelIds { get; }
}