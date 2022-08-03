namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Province
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    [ForeignKey(nameof(Climate))]
    public int ClimateId { get; set; }

    [ForeignKey(nameof(Effect))]
    public int? EffectId { get; set; }

    public Climate Climate { get; set; }

    public Effect Effect { get; set; }

    public ICollection<Region> Regions { get; set; }
}