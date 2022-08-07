namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;

public class BuildingBranch
{
    public BuildingBranch(SlotType slotType, RegionType? regionType, Resource resource, Religion religion, IEnumerable<BuildingLevel> levels)
    {
        this.SlotType = slotType;
        this.RegionType = regionType;
        this.Resource = resource;
        this.Religion = religion;
        this.Levels = levels;
    }

    public SlotType SlotType { get; }

    public RegionType? RegionType { get; }

    public Resource Resource { get; }

    public Religion Religion { get; }

    public IEnumerable<BuildingLevel> Levels { get; }
}