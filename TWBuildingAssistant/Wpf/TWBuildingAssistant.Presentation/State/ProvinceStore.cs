namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using static TWBuildingAssistant.Domain.DTOs;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevels = new Dictionary<(int RegionId, int SlotIndex), (string BuildingBranchId, string BuildingLevelId)>();
        this.SeekerResults = new List<SeekerResultDto>();
    }

    public Dictionary<(int RegionId, int SlotIndex), (string BuildingBranchId, string BuildingLevelId)> BuildingLevels { get; }

    public SeekerSettingsRegionDto[] SeekerSettings { get; set; }

    public List<SeekerResultDto> SeekerResults { get; }
}