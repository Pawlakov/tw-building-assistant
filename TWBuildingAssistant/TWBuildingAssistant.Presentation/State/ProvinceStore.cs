namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using System.Collections.Immutable;
using TWBuildingAssistant.Domain;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevels = new Dictionary<(int RegionId, int SlotIndex), (Buildings.BuildingBranch BuildingBranch, Buildings.BuildingLevel BuildingLevel)>();
        this.SeekerResults = new List<Seeker.SeekerResult>();
    }

    public Dictionary<(int RegionId, int SlotIndex), (Buildings.BuildingBranch BuildingBranch, Buildings.BuildingLevel BuildingLevel)> BuildingLevels { get; }

    public Seeker.SeekerSettingsRegion[] SeekerSettings { get; set; }

    public List<Seeker.SeekerResult> SeekerResults { get; }
}