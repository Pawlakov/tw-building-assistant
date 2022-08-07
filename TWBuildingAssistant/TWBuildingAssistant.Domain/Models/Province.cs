namespace TWBuildingAssistant.Domain.Models;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;

public class Province
{
    private readonly Effect baseEffect;
    private readonly Influence baseInfluence;

    private readonly Climate climate;

    public Province(string name, IEnumerable<Region> regions, Climate climate, Effect baseEffect = default, Influence baseInfluence = default)
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
        this.baseInfluence = baseInfluence;
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
            var corruptionEffect = EffectOperations.Create(0, 0, 0, 0, 0, 0, 0, 0, 0, new[] { IncomeOperations.Create(-this.CorruptionRate, null, BonusType.Percentage), });

            var climateEffect = this.climate.GetEffect(this.Season, this.Weather);
            var regionalEffects = this.Regions.Select(x => x.Effects);
            var regionalInfluences = this.Regions.Select(x => x.Influence);
            var effect = EffectOperations.Collect(regionalEffects.SelectMany(x => x).Append(this.baseEffect).Append(corruptionEffect).Append(climateEffect).Concat(this.Owner.FactionwideEffects));
            var influence = regionalInfluences.Aggregate(this.baseInfluence + this.Owner.FactionwideInfluence, (x, y) => x + y);

            var fertility = effect.Fertility < 0 ? 0 : effect.Fertility > 5 ? 5 : effect.Fertility;
            var sanitation = regionalEffects.Select(x => x.Sum(y => y.RegionalSanitation) + effect.ProvincialSanitation);
            var food = effect.RegularFood + (fertility * effect.FertilityDependentFood);
            var publicOrder = effect.PublicOrder + influence.PublicOrder(this.Owner.StateReligion);
            var income = IncomeOperations.Collect(effect.Incomes, fertility);

            var regionStates = sanitation.Select(x => new RegionState(x)).ToImmutableArray();
            var provinceState = new ProvinceState(regionStates, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
            return provinceState;
        }
    }
}