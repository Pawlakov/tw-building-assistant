namespace TWBuildingAssistant.Domain.OldModels;

using System.Collections.Generic;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;

public class BuildingBranch
{
    public BuildingBranch(SlotType slotType, RegionType? regionType, int? resourceId, int? religionId, IEnumerable<BuildingLevel> levels)
    {
        this.SlotType = slotType;
        this.RegionType = regionType;
        this.ResourceId = resourceId;
        this.ReligionId = religionId;
        this.Levels = levels;
    }

    public SlotType SlotType { get; }

    public RegionType? RegionType { get; }

    public int? ResourceId { get; }

    public int? ReligionId { get; }

    public IEnumerable<BuildingLevel> Levels { get; }
}