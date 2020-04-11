namespace TWBuildingAssistant.Data.Sqlite.Model
{
    public class BuildingLevel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentBuildingLevelId { get; set; }

        public int? RegionalEffectId { get; set; }
    }
}