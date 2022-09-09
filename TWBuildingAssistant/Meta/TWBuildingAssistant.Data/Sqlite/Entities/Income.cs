namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Income
{
    [Key]
    public int Id { get; set; }

    public int Value { get; set; }

    public int Category { get; set; }

    public bool IsFertilityDependent { get; set; }

    [ForeignKey(nameof(BuildingLevel))]
    public int BuildingLevelId { get; set; }

    public BuildingLevel BuildingLevel { get; set; }
}