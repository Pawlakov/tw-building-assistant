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
    private readonly ImmutableArray<Income> baseIncomes;
    private readonly Influence baseInfluence;

    private readonly Climate climate;

    public Province(string name, IEnumerable<Region> regions, Climate climate, Effect baseEffect, IEnumerable<Income> baseIncomes, Influence baseInfluence)
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
        this.baseIncomes = baseIncomes.ToImmutableArray();
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

    public ProvinceState GetState(ImmutableArray<Religion> religions)
    {
        var corruptionIncome = IncomeOperations.Create(-this.CorruptionRate, null, BonusType.Percentage);

        var climateEffect = this.climate.GetEffect(this.Season, this.Weather);
        var climateIncomes = this.climate.GetIncomes(this.Season, this.Weather);
        var regionalEffects = this.Regions.Select(x => x.Effects);
        var regionalIncomes = this.Regions.Select(x => x.Incomes);
        var regionalInfluences = this.Regions.Select(x => x.Influence);
        var effect = EffectOperations.Collect(regionalEffects.SelectMany(x => x).Append(this.baseEffect).Append(climateEffect).Concat(this.Owner.GetFactionwideEffects(religions)));
        var incomes = regionalIncomes.SelectMany(x => x).Concat(this.baseIncomes).Append(corruptionIncome).Concat(climateIncomes).Concat(this.Owner.GetFactionwideIncomes(religions));
        var influence = regionalInfluences.Aggregate(this.baseInfluence + this.Owner.GetFactionwideInfluence(religions), (x, y) => x + y);

        var fertility = effect.Fertility < 0 ? 0 : effect.Fertility > 5 ? 5 : effect.Fertility;
        var sanitation = regionalEffects.Select(x => x.Sum(y => y.RegionalSanitation) + effect.ProvincialSanitation);
        var food = effect.RegularFood + (fertility * effect.FertilityDependentFood);
        var publicOrder = effect.PublicOrder + influence.PublicOrder(this.Owner.StateReligionId.Value);
        var income = IncomeOperations.Collect(incomes, fertility);

        var regionStates = sanitation.Select(x => new RegionState(x)).ToImmutableArray();
        var provinceState = new ProvinceState(regionStates, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
        return provinceState;
    }
}