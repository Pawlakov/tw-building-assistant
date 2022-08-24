namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using System.Collections.Immutable;
using TWBuildingAssistant.Domain.StateModels;

public interface IProvinceStore
{
    Dictionary<(int RegionId, int SlotIndex), (Data.FSharp.Models.BuildingBranch BuildingBranch, Data.FSharp.Models.BuildingLevel BuildingLevel)> BuildingLevels { get; }

    ImmutableArray<SeekerSettingsRegion> SeekerSettings { get; set; }

    List<SeekerResult> SeekerResults { get; }
}