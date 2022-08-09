namespace TWBuildingAssistant.Domain.OldModels;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;
using TWBuildingAssistant.Domain.StateModels;

public class Province
{
    private readonly Effect baseEffect;
    private readonly ImmutableArray<Income> baseIncomes;
    private readonly ImmutableArray<Influence> baseInfluences;

    public Province(int id, string name, IEnumerable<Region> regions, int climateId, Effect baseEffect, IEnumerable<Income> baseIncomes, IEnumerable<Influence> baseInfluences)
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

        this.Id = id;
        this.Name = name;
        this.baseEffect = baseEffect;
        this.baseIncomes = baseIncomes.ToImmutableArray();
        this.baseInfluences = baseInfluences.ToImmutableArray();
        this.Regions = regions.ToList();
        this.ClimateId = climateId;
    }

    public int Id { get; set; }

    public string Name { get; init; }

    public int ClimateId { get; init; }

    public IEnumerable<Region> Regions { get; init; }

    public ProvinceState GetState(
        in ProvinceSettings settings,
        in FactionSettings factionSettings,
        Faction faction,
        in Climate climate,
        in Religion religion)
    {
        var corruptionIncome = IncomeOperations.Create(-settings.CorruptionRate, null, BonusType.Percentage);

        (var climateEffect, var climateIncomes) = ClimateOperations.GetEffects(climate, settings.SeasonId, settings.WeatherId);
        var regionalEffects = this.Regions.Select(x => x.Effects);
        var regionalIncomes = this.Regions.Select(x => x.Incomes);
        var regionalInfluences = this.Regions.Select(x => x.Influences);
        var effect = EffectOperations.Collect(regionalEffects.SelectMany(x => x).Append(this.baseEffect).Append(climateEffect).Concat(faction.GetFactionwideEffects(factionSettings, religion)));
        var incomes = regionalIncomes.SelectMany(x => x).Concat(this.baseIncomes).Append(corruptionIncome).Concat(climateIncomes).Concat(faction.GetFactionwideIncomes(factionSettings, religion));
        var influences = regionalInfluences.SelectMany(x => x).Concat(this.baseInfluences).Concat(faction.GetFactionwideInfluence(factionSettings, religion));

        var fertility = effect.Fertility < 0 ? 0 : effect.Fertility > 5 ? 5 : effect.Fertility;
        var sanitation = regionalEffects.Select(x => x.Sum(y => y.RegionalSanitation) + effect.ProvincialSanitation);
        var food = effect.RegularFood + (fertility * effect.FertilityDependentFood);
        var publicOrder = effect.PublicOrder + InfluenceOperations.Collect(influences, factionSettings.ReligionId);
        var income = IncomeOperations.Collect(incomes, fertility);

        var regionStates = sanitation.Select(x => new RegionState(x)).ToImmutableArray();
        var provinceState = new ProvinceState(regionStates, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
        return provinceState;
    }
}