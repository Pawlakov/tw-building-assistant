namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class WeatherEffect
{
    [ForeignKey(nameof(Season))]
    public int SeasonId { get; set; }

    [ForeignKey(nameof(Climate))]
    public int ClimateId { get; set; }

    [ForeignKey(nameof(Weather))]
    public int WeatherId { get; set; }

    [ForeignKey(nameof(Effect))]
    public int EffectId { get; set; }

    public Season Season { get; set; }

    public Climate Climate { get; set; }

    public Weather Weather { get; set; }

    public Effect Effect { get; set; }
}