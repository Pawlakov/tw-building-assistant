namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Tax
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    public int Order { get; set; }

    [ForeignKey(nameof(Effect))]
    public int? EffectId { get; set; }

    public Effect? Effect { get; set; }
}
