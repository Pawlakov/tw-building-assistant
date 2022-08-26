namespace TWBuildingAssistant.Presentation.State;

using System.Collections.Generic;
using TWBuildingAssistant.Domain;

public interface IProvinceStore
{
    Dictionary<(int RegionId, int SlotIndex), (Buildings.BuildingBranch BuildingBranch, Buildings.BuildingLevel BuildingLevel)> BuildingLevels { get; }

    Seeker.SeekerSettingsRegion[] SeekerSettings { get; set; }

    List<Seeker.SeekerResult> SeekerResults { get; }
}