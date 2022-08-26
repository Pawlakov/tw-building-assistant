namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Resource
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    public byte[]? Icon { get; set; }

    public ICollection<BuildingBranch> BuildingBranches { get; set; }

    public ICollection<Region> Regions { get; set; }
}