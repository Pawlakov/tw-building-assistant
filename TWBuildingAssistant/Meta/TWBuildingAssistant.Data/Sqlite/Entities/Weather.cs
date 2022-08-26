namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Weather
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    public int Order { get; set; }

    public ICollection<WeatherEffect> Effects { get; set; }
}