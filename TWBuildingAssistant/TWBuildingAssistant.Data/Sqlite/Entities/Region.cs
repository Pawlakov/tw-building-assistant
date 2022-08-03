namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TWBuildingAssistant.Data.Model;

public class Region
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    public RegionType RegionType { get; set; }

    public bool IsCoastal { get; set; }

    public int SlotsCountOffset { get; set; }

    [ForeignKey(nameof(Resource))]
    public int? ResourceId { get; set; }

    [ForeignKey(nameof(Province))]
    public int ProvinceId { get; set; }

    public Resource Resource { get; set; }

    public Province Province { get; set; }
}