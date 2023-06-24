namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using static TWBuildingAssistant.Domain.DTOs;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevels = new Dictionary<(string RegionId, int SlotIndex), (string BuildingBranchId, string BuildingLevelId)>();
        this.SeekerResults = new List<SeekerResultDTO>();
    }

    public Dictionary<(string RegionId, int SlotIndex), (string BuildingBranchId, string BuildingLevelId)> BuildingLevels { get; }

    public SeekerSettingsRegionDTO[] SeekerSettings { get; set; }

    public List<SeekerResultDTO> SeekerResults { get; }
}