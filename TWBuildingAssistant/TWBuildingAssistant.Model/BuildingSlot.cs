namespace TWBuildingAssistant.Model
{
    using TWBuildingAssistant.Data.Model;

    public class BuildingSlot
    {
        public BuildingSlot(SlotType slotType, RegionType regionType)
        {
            this.SlotType = slotType;
            this.RegionType = regionType;
        }

        public SlotType SlotType { get; }

        public RegionType RegionType { get; }

        public Effect Effect => default;
    }
}