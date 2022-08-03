namespace TWBuildingAssistant.Model;

using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Data.Model;

public class Province
{
    private readonly Effect baseEffect;

    private readonly Climate climate;

    public Province(string name, IEnumerable<Region> regions, Climate climate, Effect baseEffect = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Province without name.");
        }

        if (regions == null)
        {
            throw new DomainRuleViolationException("No regions given for a province.");
        }

        if (regions.Count() != 3)
        {
            throw new DomainRuleViolationException("Invalid region count.");
        }

        if (climate == null)
        {
            throw new DomainRuleViolationException("Province without climate.");
        }

        this.Name = name;
        this.baseEffect = baseEffect;
        this.Regions = regions.ToList();
        this.climate = climate;
    }

    public string Name { get; }

    public IEnumerable<Region> Regions { get; }

    public Faction Owner { get; set; }

    public Weather Weather { get; set; }

    public Season Season { get; set; }

    public int CorruptionRate { get; set; }

    public ProvinceState State
    {
        get
        {
            var corruptionEffect = new Effect(0, 0, 0, 0, 0, 0, 0, 0, 0, new Income(-this.CorruptionRate, null, BonusType.Percentage), default);

            var climateEffect = this.climate.GetEffect(this.Season, this.Weather);
            var regionalEffects = this.Regions.Select(x => x.Effect);
            var effect = regionalEffects.Aggregate(this.baseEffect + corruptionEffect + climateEffect + this.Owner.FactionwideEffect, (x, y) => x + y);

            var fertility = effect.Fertility < 0 ? 0 : (effect.Fertility > 5 ? 5 : effect.Fertility);
            var sanitation = regionalEffects.Select(x => x.RegionalSanitation + effect.ProvincialSanitation);
            var food = effect.RegularFood + (fertility * effect.FertilityDependentFood);
            var publicOrder = effect.PublicOrder + effect.Influence.PublicOrder(this.Owner.StateReligion);
            var income = effect.Income.GetIncome(fertility);

            return new ProvinceState(sanitation, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
        }
    }
}