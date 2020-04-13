namespace TWBuildingAssistant.Model.Combinations
{
    public class BuildingSlot
    {
        public BuildingSlot(Buildings.SlotType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return string.Format
                (
                    "Type: {0} Building: {1}",
                    Type,
                    Level != null ? Level.ToString() : "???"
                );
        }

        public Buildings.SlotType Type { get; }

        public Buildings.Building Level { get; set; }
    }
}
