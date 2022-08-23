namespace TWBuildingAssistant.Domain.Services;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public class ProvinceService
    : IProvinceService
{
    public ProvinceState GetState(
        IEnumerable<IEnumerable<BuildingLevel>> buildings,
        Data.FSharp.Models.Settings settings,
        Data.FSharp.Models.EffectSet predefinedEffect)
    {
        (var regionalEffects, var regionalIncomes, var regionalInfluences) = this.GetStateFromBuildings(buildings);

        var effect = Data.FSharp.Library.collectEffectsSeq(regionalEffects.Append(predefinedEffect.Effect));
        var incomes = regionalIncomes.Concat(predefinedEffect.Incomes);
        var influences = regionalInfluences.Concat(predefinedEffect.Influences);

        var fertility = effect.Fertility < 0 ? 0 : effect.Fertility > 5 ? 5 : effect.Fertility;
        var sanitation = regionalEffects.Select(x => x.RegionalSanitation + effect.ProvincialSanitation);
        var food = effect.RegularFood + (fertility * effect.FertilityDependentFood);
        var publicOrder = effect.PublicOrder + Data.FSharp.Library.collectInfluencesSeq(settings.ReligionId, influences);
        var income = Data.FSharp.Library.collectIncomesSeq(fertility, incomes);

        var regionStates = sanitation.Select(x => new RegionState(x)).ToImmutableArray();
        var provinceState = new ProvinceState(regionStates, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
        return provinceState;
    }

    private (ImmutableArray<Data.FSharp.Models.Effect> RegionalEffects, ImmutableArray<Data.FSharp.Models.Income> Incomes, ImmutableArray<Data.FSharp.Models.Influence> Influences) GetStateFromBuildings(
        IEnumerable<IEnumerable<BuildingLevel>> buildings)
    {
        var regionalEffects = buildings.Select(x => Data.FSharp.Library.collectEffectsSeq(x.Select(x => x.Effect))).ToImmutableArray();
        var regionalIncomes = buildings.SelectMany(x => x).SelectMany(x => x.Incomes).ToImmutableArray();
        var regionalInfluences = buildings.SelectMany(x => x).SelectMany(x => x.Influences).ToImmutableArray();

        return (regionalEffects, regionalIncomes, regionalInfluences);
    }
}