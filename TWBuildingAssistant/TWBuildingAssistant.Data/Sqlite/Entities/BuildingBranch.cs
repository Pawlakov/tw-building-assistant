namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    using TWBuildingAssistant.Data.Model;

    public class BuildingBranch
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public SlotType SlotType { get; set; }

        public RegionType? RegionType { get; set; }

        public bool AllowParallel { get; set; }

        public int RootBuildingLevelId { get; set; }

        public int? ReligionId { get; set; }

        public int? ResourceId { get; set; }
    }
}