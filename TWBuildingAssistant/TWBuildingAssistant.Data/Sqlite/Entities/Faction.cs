namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Faction
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public ICollection<BuildingBranchUse> BuildingBranchesUsed { get; set; }

        public ICollection<TechnologyLevel> TechnologyLevels { get; set; }
    }
}