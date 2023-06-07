namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BuildingLevel
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    public int Maintenance { get; set; }

    public int LocalFood { get; set; }

    public int LocalFoodFromFertility { get; set; }

    public int LocalSanitation { get; set; }

    public int CapitalTier { get; set; }

    [ForeignKey(nameof(Parent))]
    public int? ParentBuildingLevelId { get; set; }

    [ForeignKey(nameof(Effect))]
    public int? EffectId { get; set; }

    public BuildingLevel? Parent { get; set; }

    public Effect? Effect { get; set; }

    public BuildingBranch? BuildingBranch { get; set; }

    public ICollection<BuildingLevelLock> Locks { get; set; }

    public ICollection<Income> Incomes { get; set; }
}