namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Domain.OldModels;
using TWBuildingAssistant.Domain.StateModels;

public class ProvinceService
    : IProvinceService
{
    private readonly DatabaseContextFactory contextFactory;

    public ProvinceService(DatabaseContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public ProvinceState GetState(
        Province province,
        in Settings settings,
        Effect predefinedEffect,
        ImmutableArray<Income> predefinedIncomes,
        ImmutableArray<Influence> predefinedInfluences)
    {
        (var regionalEffects, var regionalIncomes, var regionalInfluences) = this.GetStateFromBuildings(province.Regions.Select(x => x.Slots.Select(x => x.Building)));

        var effect = EffectOperations.Collect(regionalEffects.Append(predefinedEffect));
        var incomes = regionalIncomes.Concat(predefinedIncomes);
        var influences = regionalInfluences.Concat(predefinedInfluences);

        var fertility = effect.Fertility < 0 ? 0 : effect.Fertility > 5 ? 5 : effect.Fertility;
        var sanitation = regionalEffects.Select(x => x.RegionalSanitation + effect.ProvincialSanitation);
        var food = effect.RegularFood + (fertility * effect.FertilityDependentFood);
        var publicOrder = effect.PublicOrder + InfluenceOperations.Collect(influences, settings.ReligionId);
        var income = IncomeOperations.Collect(incomes, fertility);

        var regionStates = sanitation.Select(x => new RegionState(x)).ToImmutableArray();
        var provinceState = new ProvinceState(regionStates, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
        return provinceState;
    }

    public (Effect Effect, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences) GetStateFromSettings(
        Province province,
        in Settings settings,
        Faction faction,
        in Climate climate,
        in Religion religion)
    {
        var corruptionIncome = IncomeOperations.Create(-settings.CorruptionRate, null, BonusType.Percentage);

        (var climateEffect, var climateIncomes) = ClimateOperations.GetEffects(climate, settings.SeasonId, settings.WeatherId);
        var effect = EffectOperations.Collect(faction.GetFactionwideEffects(settings, religion).Append(province.BaseEffect).Append(climateEffect));
        var incomes = province.BaseIncomes.Append(corruptionIncome).Concat(climateIncomes).Concat(faction.GetFactionwideIncomes(settings, religion));
        var influences = province.BaseInfluences.Concat(faction.GetFactionwideInfluence(settings, religion));

        return (effect, incomes.ToImmutableArray(), influences.ToImmutableArray());
    }

    private (ImmutableArray<Effect> RegionalEffects, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences) GetStateFromBuildings(
        IEnumerable<IEnumerable<BuildingLevel>> buildings)
    {
        var regionalEffects = buildings.Select(x => EffectOperations.Collect(x.Select(x => x.Effect)));
        var regionalIncomes = buildings.SelectMany(x => x).SelectMany(x => x.Incomes);
        var regionalInfluences = buildings.SelectMany(x => x).SelectMany(x => x.Influences);

        return (regionalEffects.ToImmutableArray(), regionalIncomes.ToImmutableArray(), regionalInfluences.ToImmutableArray());
    }
}