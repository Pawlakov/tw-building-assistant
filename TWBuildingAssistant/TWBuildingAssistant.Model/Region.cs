namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.Model;

    public class Region
    {
        private const int CitySlotCount = 6;

        private const int TownSlotCount = 4;

        public Region(string name, RegionType type, bool isCoastal, Resource resource = null, bool missingSlot = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainRuleViolationException("Region without name.");
            }

            this.Name = name;
            this.Resource = resource;

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

        public string Name { get; }

        public Resource Resource { get; }

        public IEnumerable<BuildingSlot> Slots { get; }

        public int Sanitation => throw new NotImplementedException();
    }
}