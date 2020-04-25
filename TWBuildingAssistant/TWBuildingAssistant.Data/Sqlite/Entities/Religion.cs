namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Religion
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [ForeignKey(nameof(Effect))]
        public int? EffectId { get; set; }

        public Effect Effect { get; set; }

        public ICollection<BuildingBranch> BuildingBranches { get; set; }

        public ICollection<Influence> Influences { get; set; }
    }
}