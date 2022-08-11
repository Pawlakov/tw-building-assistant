namespace TWBuildingAssistant.Domain;

using System.Collections.Generic;
using System.Collections.Immutable;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain.Exceptions;

public readonly record struct Region(int Id, string Name, RegionType RegionType, int? ResourceId, ImmutableArray<SlotDescriptor> Slots);

public static class RegionOperations
{
    public const int CitySlotCount = 6;

    public const int TownSlotCount = 4;

    public static Region Create(int id, string name, RegionType type, bool isCoastal, int? resourceId, bool missingSlot)
    {
        if (id == 0)
        {
            throw new DomainRuleViolationException("Region without id.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Region without name.");
        }

        var slotCount = (type == RegionType.City ? CitySlotCount : TownSlotCount) + (missingSlot ? -1 : 0);
        var slots = new List<SlotDescriptor>();
        slots.Add(new SlotDescriptor(SlotType.Main, type, resourceId));
        slots.Add(new SlotDescriptor(isCoastal ? SlotType.Coastal : SlotType.General, type, resourceId));
        for (var i = 2; i < slotCount; ++i)
        {
            slots.Add(new SlotDescriptor(SlotType.General, type, resourceId));
        }

        return new Region(id, name, type, resourceId, slots.ToImmutableArray());
    }
}