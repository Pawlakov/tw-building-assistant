namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Bonus
{
    [Key]
    public int Id { get; set; }

    public int Value { get; set; }

    public int? Category { get; set; }

    [ForeignKey(nameof(Effect))]
    public int? EffectId { get; set; }

    public Effect Effect { get; set; }
}