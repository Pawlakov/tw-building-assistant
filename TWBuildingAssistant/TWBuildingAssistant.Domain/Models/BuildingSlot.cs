namespace TWBuildingAssistant.Domain.Models;

using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain.Exceptions;

public class BuildingSlot
{
    private BuildingLevel buildingLevel;

    public BuildingSlot(SlotType slotType, RegionType regionType)
    {
        this.SlotType = slotType;
        this.RegionType = regionType;
    }

    public SlotType SlotType { get; }

    public RegionType RegionType { get; }

    public BuildingLevel Building
    {
        get => this.buildingLevel;
        set
        {
            if (value == null)
            {
                throw new DomainRuleViolationException("Null building level.");
            }

            this.buildingLevel = value;
        }
    }

    public Effect Effect => this.buildingLevel.Effect;
}