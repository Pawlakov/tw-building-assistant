namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    public class BuildingLevel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentBuildingLevelId { get; set; }

        public int? EffectId { get; set; }
    }
}