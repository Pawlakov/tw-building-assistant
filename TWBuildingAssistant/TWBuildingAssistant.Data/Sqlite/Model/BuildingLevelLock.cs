namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class BuildingLevelLock : IBuildingLevelLock
    {
        public BuildingLevelLock()
        {
        }

        public BuildingLevelLock(IBuildingLevelLock source)
        {
            this.BuildingLevelId = source.BuildingLevelId;
            this.TechnologyLevelId = source.TechnologyLevelId;
            this.Antilegacy = source.Antilegacy;
            this.Lock = source.Lock;
        }

        public int BuildingLevelId { get; set; }

        public int TechnologyLevelId { get; set; }

        public bool Antilegacy { get; set; }

        public bool Lock { get; set; }
    }
}