namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using System.Collections.Immutable;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevels = new Dictionary<(int RegionId, int SlotIndex), (BuildingBranch BuildingBranch, BuildingLevel BuildingLevel)>();
        this.SeekerResults = new List<SeekerResult>();
    }

    public Dictionary<(int RegionId, int SlotIndex), (BuildingBranch BuildingBranch, BuildingLevel BuildingLevel)> BuildingLevels { get; }

    public ImmutableArray<SeekerSettingsRegion> SeekerSettings { get; set; }

    public List<SeekerResult> SeekerResults { get; }
}