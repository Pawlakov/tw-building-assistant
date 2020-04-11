namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class BuildingBranch : IBuildingBranch
    {
        public BuildingBranch()
        {
        }

        public BuildingBranch(IBuildingBranch source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
            this.SlotType = source.SlotType;
            this.RegionType = source.RegionType;
            this.AllowParallel = source.AllowParallel;
            this.RootBuildingLevelId = source.RootBuildingLevelId;
            this.ReligionId = source.ReligionId;
            this.ResourceId = source.ResourceId;
        }

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