namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using static TWBuildingAssistant.Domain.DTOs;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevels = new Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)>();
        this.SeekerResults = new List<SeekerResultDto>();
    }

    public Dictionary<(int RegionId, int SlotIndex), (int BuildingBranchId, int BuildingLevelId)> BuildingLevels { get; }

    public SeekerSettingsRegionDto[] SeekerSettings { get; set; }

    public List<SeekerResultDto> SeekerResults { get; }
}