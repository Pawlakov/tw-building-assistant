namespace TWBuildingAssistant.Domain.OldModels;

using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;

public class Region
{
    private const int CitySlotCount = 6;

    private const int TownSlotCount = 4;

    public Region(int id, string name, RegionType type, bool isCoastal, int? resourceId = default, bool missingSlot = false)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Region without name.");
        }

        this.Id = id;
        this.Name = name;
        this.ResourceId = resourceId;

        var slotCount = (type == RegionType.City ? CitySlotCount : TownSlotCount) + (missingSlot ? -1 : 0);
        var slots = new List<BuildingSlot>();
        slots.Add(new BuildingSlot(SlotType.Main, type));
        slots.Add(new BuildingSlot(isCoastal ? SlotType.Coastal : SlotType.General, type));
        for (var i = 2; i < slotCount; ++i)
        {
            slots.Add(new BuildingSlot(SlotType.General, type));
        }

        this.Slots = slots;
    }

    public int Id { get; }

    public string Name { get; }

    public int? ResourceId { get; }

    public IEnumerable<BuildingSlot> Slots { get; }
}