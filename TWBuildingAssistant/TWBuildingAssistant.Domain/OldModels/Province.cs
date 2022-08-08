namespace TWBuildingAssistant.Domain.OldModels;

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
    private readonly ImmutableArray<Influence> baseInfluences;

    private readonly int climateId;

    public Province(string name, IEnumerable<Region> regions, int climateId, Effect baseEffect, IEnumerable<Income> baseIncomes, IEnumerable<Influence> baseInfluences)
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

        this.Name = name;
        this.baseEffect = baseEffect;
        this.baseIncomes = baseIncomes.ToImmutableArray();
        this.baseInfluences = baseInfluences.ToImmutableArray();
        this.Regions = regions.ToList();
        this.climateId = climateId;
    }

    public string Name { get; }

    public IEnumerable<Region> Regions { get; }

    public Faction Owner { get; set; }

    public int WeatherId { get; set; }

    public int SeasonId { get; set; }

    public int CorruptionRate { get; set; }

    public ProvinceState GetState(ImmutableArray<Climate> climates, ImmutableArray<Religion> religions)
    {
        var corruptionIncome = IncomeOperations.Create(-this.CorruptionRate, null, BonusType.Percentage);

        var climate = climates.Single(x => x.Id == this.climateId);
        (var climateEffect, var climateIncomes) = ClimateOperations.GetEffects(climate, this.SeasonId, this.WeatherId);
        var regionalEffects = this.Regions.Select(x => x.Effects);
        var regionalIncomes = this.Regions.Select(x => x.Incomes);
        var regionalInfluences = this.Regions.Select(x => x.Influences);
        var effect = EffectOperations.Collect(regionalEffects.SelectMany(x => x).Append(this.baseEffect).Append(climateEffect).Concat(this.Owner.GetFactionwideEffects(religions)));
        var incomes = regionalIncomes.SelectMany(x => x).Concat(this.baseIncomes).Append(corruptionIncome).Concat(climateIncomes).Concat(this.Owner.GetFactionwideIncomes(religions));
        var influences = regionalInfluences.SelectMany(x => x).Concat(this.baseInfluences).Concat(this.Owner.GetFactionwideInfluence(religions));

        var fertility = effect.Fertility < 0 ? 0 : effect.Fertility > 5 ? 5 : effect.Fertility;
        var sanitation = regionalEffects.Select(x => x.Sum(y => y.RegionalSanitation) + effect.ProvincialSanitation);
        var food = effect.RegularFood + (fertility * effect.FertilityDependentFood);
        var publicOrder = effect.PublicOrder + InfluenceOperations.Collect(influences, this.Owner.StateReligionId.Value);
        var income = IncomeOperations.Collect(incomes, fertility);

        var regionStates = sanitation.Select(x => new RegionState(x)).ToImmutableArray();
        var provinceState = new ProvinceState(regionStates, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
        return provinceState;
    }
}