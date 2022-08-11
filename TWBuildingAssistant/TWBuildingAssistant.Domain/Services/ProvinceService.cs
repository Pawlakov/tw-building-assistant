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
    private readonly DatabaseContextFactory contextFactory;

    public ProvinceService(DatabaseContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<Province> GetProvince(int provinceId)
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entity = await context.Provinces
                .AsNoTracking()
                .Include(x => x.Regions)
                .Where(x => x.Id == provinceId)
                .FirstOrDefaultAsync();

            return ProvinceOperations.Create(entity.Id, entity.Name, entity.Regions.Select(x => RegionOperations.Create(x.Id, x.Name, x.RegionType, x.IsCoastal, x.ResourceId, x.SlotsCountOffset != 0)));
        }
    }

    public ProvinceState GetState(
        IEnumerable<IEnumerable<BuildingLevel>> buildings,
        in Settings settings,
        Effect predefinedEffect,
        ImmutableArray<Income> predefinedIncomes,
        ImmutableArray<Influence> predefinedInfluences)
    {
        (var regionalEffects, var regionalIncomes, var regionalInfluences) = this.GetStateFromBuildings(buildings);

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

    private (ImmutableArray<Effect> RegionalEffects, ImmutableArray<Income> Incomes, ImmutableArray<Influence> Influences) GetStateFromBuildings(
        IEnumerable<IEnumerable<BuildingLevel>> buildings)
    {
        var regionalEffects = buildings.Select(x => EffectOperations.Collect(x.Select(x => x.Effect))).ToImmutableArray();
        var regionalIncomes = buildings.SelectMany(x => x).SelectMany(x => x.Incomes).ToImmutableArray();
        var regionalInfluences = buildings.SelectMany(x => x).SelectMany(x => x.Influences).ToImmutableArray();

        return (regionalEffects, regionalIncomes, regionalInfluences);
    }
}