namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using TWBuildingAssistant.Domain;
using static TWBuildingAssistant.Domain.Interface;

public interface IProvinceStore
{
    Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevels { get; }

    SeekerSettingsRegionDto[] SeekerSettings { get; set; }

    List<SeekerResultDto> SeekerResults { get; }
}