namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using System.Collections.Immutable;

public class ProvinceStore
    : IProvinceStore
{
    public ProvinceStore()
    {
        this.BuildingLevels = new Dictionary<(int RegionId, int SlotIndex), (Data.FSharp.Models.BuildingBranch BuildingBranch, Data.FSharp.Models.BuildingLevel BuildingLevel)>();
        this.SeekerResults = new List<Data.FSharp.Models.SeekerResult>();
    }

    public Dictionary<(int RegionId, int SlotIndex), (Data.FSharp.Models.BuildingBranch BuildingBranch, Data.FSharp.Models.BuildingLevel BuildingLevel)> BuildingLevels { get; }

    public Data.FSharp.Models.SeekerSettingsRegion[] SeekerSettings { get; set; }

    public List<Data.FSharp.Models.SeekerResult> SeekerResults { get; }
}