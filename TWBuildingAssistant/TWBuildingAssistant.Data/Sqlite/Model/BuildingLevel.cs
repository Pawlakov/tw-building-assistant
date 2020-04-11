namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class BuildingLevel : IBuildingLevel
    {
        public BuildingLevel()
        {
        }

        public BuildingLevel(IBuildingLevel source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
            this.ParentBuildingLevelId = source.ParentBuildingLevelId;
            this.RegionalEffectId = source.RegionalEffectId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentBuildingLevelId { get; set; }

        public int? RegionalEffectId { get; set; }
    }
}