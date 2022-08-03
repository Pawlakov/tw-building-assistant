namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Influence
{
    [Key]
    public int Id { get; set; }

    public int Value { get; set; }

    [ForeignKey(nameof(Religion))]
    public int? ReligionId { get; set; }

    [ForeignKey(nameof(Effect))]
    public int? EffectId { get; set; }

    public Religion Religion { get; set; }

    public Effect Effect { get; set; }
}