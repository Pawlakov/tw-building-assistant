namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using static TWBuildingAssistant.Domain.DTOs;

public interface IProvinceStore
{
    Dictionary<(string RegionId, int SlotIndex), (string BuildingBranchId, string BuildingLevelId)> BuildingLevels { get; }

    SeekerSettingsRegionDTO[] SeekerSettings { get; set; }

    List<SeekerResultDTO> SeekerResults { get; }
}