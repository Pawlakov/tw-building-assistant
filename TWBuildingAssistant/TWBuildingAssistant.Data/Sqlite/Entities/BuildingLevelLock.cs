namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class BuildingLevelLock
    {
        [ForeignKey(nameof(BuildingLevel))]
        public int BuildingLevelId { get; set; }

        [ForeignKey(nameof(TechnologyLevel))]
        public int TechnologyLevelId { get; set; }

        public bool Antilegacy { get; set; }

        public bool Lock { get; set; }

        public BuildingLevel BuildingLevel { get; set; }

        public TechnologyLevel TechnologyLevel { get; set; }
    }
}