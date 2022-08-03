namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Faction
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    [ForeignKey(nameof(Effect))]
    public int? EffectId { get; set; }

    public Effect? Effect { get; set; }

    public ICollection<BuildingBranchUse> BuildingBranchesUsed { get; set; }

    public ICollection<TechnologyLevel> TechnologyLevels { get; set; }
}