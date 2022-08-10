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
        Faction faction,
        in Climate climate,
        in Religion religion)
    {
        (var predefinedEffect, var predefinedIncomes, var predefinedInfluences) = this.GetPredefinded(province, settings, faction, climate, religion);
        var provinceState = this.GetCustomizable(province, settings, predefinedEffect, predefinedIncomes, predefinedInfluences);
        return provinceState;
    }

    private (Effect Effect, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences) GetPredefinded(
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

    private ProvinceState GetCustomizable(
        Province province,
        in Settings settings,
        in Effect predefinedEffect,
        in ImmutableArray<Income> predefinedIncomes,
        in ImmutableArray<Influence> predefinedInfluences)
    {
        var regionalEffects = province.Regions.Select(x => x.Effects);
        var regionalIncomes = province.Regions.Select(x => x.Incomes);
        var regionalInfluences = province.Regions.Select(x => x.Influences);

        var effect = EffectOperations.Collect(regionalEffects.SelectMany(x => x).Append(predefinedEffect));
        var incomes = regionalIncomes.SelectMany(x => x).Concat(predefinedIncomes);
        var influences = regionalInfluences.SelectMany(x => x).Concat(predefinedInfluences);

        var fertility = effect.Fertility < 0 ? 0 : effect.Fertility > 5 ? 5 : effect.Fertility;
        var sanitation = regionalEffects.Select(x => x.Sum(y => y.RegionalSanitation) + effect.ProvincialSanitation);
        var food = effect.RegularFood + (fertility * effect.FertilityDependentFood);
        var publicOrder = effect.PublicOrder + InfluenceOperations.Collect(influences, settings.ReligionId);
        var income = IncomeOperations.Collect(incomes, fertility);

        var regionStates = sanitation.Select(x => new RegionState(x)).ToImmutableArray();
        var provinceState = new ProvinceState(regionStates, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
        return provinceState;
    }
}