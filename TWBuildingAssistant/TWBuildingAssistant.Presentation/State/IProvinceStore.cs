namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;

public interface IProvinceStore
{
    Dictionary<(int RegionId, int SlotIndex), (Domain.Models.BuildingBranch BuildingBranch, Domain.Models.BuildingLevel BuildingLevel)> BuildingLevels { get; }

    Domain.Models.SeekerSettingsRegion[] SeekerSettings { get; set; }

    List<Domain.Models.SeekerResult> SeekerResults { get; }
}