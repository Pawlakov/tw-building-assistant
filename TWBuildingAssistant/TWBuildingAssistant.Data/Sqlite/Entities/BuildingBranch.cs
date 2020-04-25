namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using TWBuildingAssistant.Data.Model;

    public class BuildingBranch
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public SlotType SlotType { get; set; }

        public RegionType? RegionType { get; set; }

        public bool AllowParallel { get; set; }

        [ForeignKey(nameof(RootBuildingLevel))]
        public int RootBuildingLevelId { get; set; }

        [ForeignKey(nameof(Religion))]
        public int? ReligionId { get; set; }

        [ForeignKey(nameof(Resource))]
        public int? ResourceId { get; set; }

        public BuildingLevel RootBuildingLevel { get; set; }

        public Religion Religion { get; set; }

        public Resource Resource { get; set; }

        public ICollection<BuildingBranchUse> Uses { get; set; }
    }
}