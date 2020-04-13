namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    public class BuildingLevelLock
    {
        public int BuildingLevelId { get; set; }

        public int TechnologyLevelId { get; set; }

        public bool Antilegacy { get; set; }

        public bool Lock { get; set; }
    }
}