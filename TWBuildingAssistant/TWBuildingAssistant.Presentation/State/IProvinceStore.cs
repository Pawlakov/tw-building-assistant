namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;

public interface IProvinceStore
{
    Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevelIds { get; }
}