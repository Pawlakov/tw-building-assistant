namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using System.Collections.Immutable;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevels = new Dictionary<(int RegionId, int SlotIndex), (Domain.Models.BuildingBranch BuildingBranch, Domain.Models.BuildingLevel BuildingLevel)>();
        this.SeekerResults = new List<Domain.Models.SeekerResult>();
    }

    public Dictionary<(int RegionId, int SlotIndex), (Domain.Models.BuildingBranch BuildingBranch, Domain.Models.BuildingLevel BuildingLevel)> BuildingLevels { get; }

    public Domain.Models.SeekerSettingsRegion[] SeekerSettings { get; set; }

    public List<Domain.Models.SeekerResult> SeekerResults { get; }
}