namespace TWBuildingAssistant.Data.Sqlite.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Effect
{
    [Key]
    public int Id { get; set; }

    public int PublicOrder { get; set; }

    public int Food { get; set; }

    public int Sanitation { get; set; }

    public int ResearchRate { get; set; }

    public int Growth { get; set; }

    public int Fertility { get; set; }

    public int ReligiousOsmosis { get; set; }

    public BuildingLevel BuildingLevel { get; set; }

    public Province Province { get; set; }

    public Religion Religion { get; set; }

    [InverseProperty(nameof(TechnologyLevel.UniversalEffect))]
    public TechnologyLevel UniversalTechnologyLevel { get; set; }

    [InverseProperty(nameof(TechnologyLevel.AntilegacyEffect))]
    public TechnologyLevel AntilegacyTechnologyLevel { get; set; }

    public WeatherEffect WeatherEffect { get; set; }

    public Faction Faction { get; set; }

    public Difficulty Difficulty { get; set; }

    public Tax Tax { get; set; }

    public ICollection<Bonus> Bonuses { get; set; }

    public ICollection<Influence> Influences { get; set; }
}