namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;

public interface IProvinceStore
{
    Dictionary<(int RegionId, int SlotIndex), (Data.FSharp.Models.BuildingBranch BuildingBranch, Data.FSharp.Models.BuildingLevel BuildingLevel)> BuildingLevels { get; }

    Data.FSharp.Models.SeekerSettingsRegion[] SeekerSettings { get; set; }

    List<Data.FSharp.Models.SeekerResult> SeekerResults { get; }
}