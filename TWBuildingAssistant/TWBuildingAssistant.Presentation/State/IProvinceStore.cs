namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using static TWBuildingAssistant.Domain.DTOs;

public interface IProvinceStore
{
    Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevels { get; }

    SeekerSettingsRegionDto[] SeekerSettings { get; set; }

    List<SeekerResultDto> SeekerResults { get; }
}