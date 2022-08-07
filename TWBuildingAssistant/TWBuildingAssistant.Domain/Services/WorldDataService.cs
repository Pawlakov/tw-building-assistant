namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;
using TWBuildingAssistant.Domain.Models;

public class WorldDataService
    : IWorldDataService
{
    private readonly DatabaseContextFactory contextFactory;

    public WorldDataService(DatabaseContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
        using (var context = this.contextFactory.CreateDbContext())
        {
            var resources = new List<KeyValuePair<int, Resource>>();
            foreach (var resourceEntity in context.Resources.ToList())
            {
                resources.Add(new KeyValuePair<int, Resource>(resourceEntity.Id, ResourceOperations.Create(resourceEntity.Name)));
            }

            var weathers = new List<KeyValuePair<int, Weather>>();
            foreach (var weatherEntity in context.Weathers.OrderBy(x => x.Order).ToList())
            {
                weathers.Add(new KeyValuePair<int, Weather>(weatherEntity.Id, WeatherOperations.Create(weatherEntity.Name)));
            }

            var seasons = new List<KeyValuePair<int, Season>>();
            foreach (var seasonEntity in context.Seasons.OrderBy(x => x.Order).ToList())
            {
                seasons.Add(new KeyValuePair<int, Season>(seasonEntity.Id, SeasonOperations.Create(seasonEntity.Name)));
            }

            var religions = new List<KeyValuePair<int, Religion>>();
            foreach (var religionEntity in context.Religions.ToList())
            {
                Effect effect = default;
                IEnumerable<Income> incomes = Enumerable.Empty<Income>();
                Influence influence = default;
                if (religionEntity.EffectId.HasValue)
                {
                    var effectEntity = context.Effects.Find(religionEntity.EffectId.Value);
                    var bonusEntities = context.Bonuses.Where(x => x.EffectId == effectEntity.Id).ToList();
                    var influenceEntities = context.Influences.Where(x => x.EffectId == effectEntity.Id).ToList();
                    if (influenceEntities.Any(x => x.ReligionId.HasValue))
                    {
                        throw new DomainRuleViolationException("Influence of a religion with a set religion id.");
                    }

                    effect = EffectOperations.Create(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, 0);
                    incomes = bonusEntities.Select(x => IncomeOperations.Create(x.Value, x.Category, x.Type));
                    influence = influenceEntities.Select(x => new Influence(null, x.Value)).Aggregate(default(Influence), (x, y) => x + y);
                }

                religions.Add(new KeyValuePair<int, Religion>(religionEntity.Id, new Religion(religionEntity.Name, effect, incomes, influence)));
            }

            var climates = new List<KeyValuePair<int, Climate>>();
            foreach (var climateEntity in context.Climates.ToList())
            {
                var weatherEffects = new Dictionary<Season, IDictionary<Weather, Effect>>();
                var weatherIncomes = new Dictionary<Season, IDictionary<Weather, IEnumerable<Income>>>();
                foreach (var season in seasons)
                {
                    var weatherEntry = new Dictionary<Weather, Effect>();
                    var incomesEntry = new Dictionary<Weather, IEnumerable<Income>>();
                    foreach (var weather in weathers)
                    {
                        var weatherEffectEntity = context.WeatherEffects.SingleOrDefault(x => x.SeasonId == season.Key && x.ClimateId == climateEntity.Id && x.WeatherId == weather.Key);
                        var effect = MakeEffect(context, weatherEffectEntity?.EffectId);
                        var incomes = MakeIncomes(context, weatherEffectEntity?.EffectId);
                        weatherEntry.Add(weather.Value, effect);
                        incomesEntry.Add(weather.Value, incomes);
                    }

                    weatherEffects.Add(season.Value, weatherEntry);
                    weatherIncomes.Add(season.Value, incomesEntry);
                }

                climates.Add(new KeyValuePair<int, Climate>(climateEntity.Id, new Climate(weatherEffects, weatherIncomes)));
            }

            var provinces = new List<KeyValuePair<int, Province>>();
            foreach (var provinceEntity in context.Provinces.ToList())
            {
                var effect = MakeEffect(context, provinceEntity.EffectId);
                var incomes = MakeIncomes(context, provinceEntity.EffectId);
                var influence = MakeInfluence(context, religions, provinceEntity.EffectId);
                var regions = new List<Region>();
                foreach (var regionEntity in context.Regions.Where(x => x.ProvinceId == provinceEntity.Id).ToList())
                {
                    regions.Add(new Region(regionEntity.Name, regionEntity.RegionType, regionEntity.IsCoastal, regionEntity.ResourceId.HasValue ? resources.Single(x => x.Key == regionEntity.ResourceId).Value : default, regionEntity.SlotsCountOffset == -1));
                }

                provinces.Add(new KeyValuePair<int, Province>(provinceEntity.Id, new Province(provinceEntity.Name, regions, climates.Single(x => x.Key == provinceEntity.ClimateId).Value, effect, incomes, influence)));
            }

            var buildings = new List<KeyValuePair<int, Tuple<BuildingLevel, int?>>>();
            foreach (var buildingLevelEntity in context.BuildingLevels)
            {
                var effect = MakeEffect(context, buildingLevelEntity.EffectId);
                var incomes = MakeIncomes(context, buildingLevelEntity.EffectId);
                var influence = MakeInfluence(context, religions, buildingLevelEntity.EffectId);
                buildings.Add(new KeyValuePair<int, Tuple<BuildingLevel, int?>>(buildingLevelEntity.Id, Tuple.Create(new BuildingLevel(buildingLevelEntity.Name, effect, incomes, influence), buildingLevelEntity.ParentBuildingLevelId)));
            }

            var branches = new List<KeyValuePair<int, BuildingBranch>>();
            foreach (var branchEntity in context.BuildingBranches)
            {
                var strains = new List<IEnumerable<BuildingLevel>>();
                void RecursiveBranchTraversing(IEnumerable<BuildingLevel> ancestors, KeyValuePair<int, Tuple<BuildingLevel, int?>> current)
                {
                    var branchLevels = ancestors.Append(current.Value.Item1);
                    var children = buildings.Where(x => x.Value.Item2 == current.Key);
                    if (children.Any())
                    {
                        foreach (var child in children)
                        {
                            RecursiveBranchTraversing(branchLevels, child);
                        }
                    }
                    else
                    {
                        strains.Add(branchLevels);
                    }
                }

                RecursiveBranchTraversing(new List<BuildingLevel>(), buildings.Single(x => x.Key == branchEntity.RootBuildingLevelId));
                if (branchEntity.AllowParallel)
                {
                    foreach (var branchLevels in strains)
                    {
                        branches.Add(new KeyValuePair<int, BuildingBranch>(branchEntity.Id, new BuildingBranch(branchEntity.SlotType, branchEntity.RegionType, resources.SingleOrDefault(x => x.Key == branchEntity.ResourceId).Value, religions.SingleOrDefault(x => x.Key == branchEntity.ReligionId).Value, branchLevels)));
                    }
                }
                else
                {
                    var branchLevels = strains.SelectMany(x => x).Distinct();
                    branches.Add(new KeyValuePair<int, BuildingBranch>(branchEntity.Id, new BuildingBranch(branchEntity.SlotType, branchEntity.RegionType, resources.SingleOrDefault(x => x.Key == branchEntity.ResourceId).Value, religions.SingleOrDefault(x => x.Key == branchEntity.ReligionId).Value, branchLevels)));
                }
            }

            var factions = new List<KeyValuePair<int, Faction>>();
            foreach (var factionEntity in context.Factions.ToList())
            {
                var effect = MakeEffect(context, factionEntity.EffectId);
                var incomes = MakeIncomes(context, factionEntity.EffectId);
                var influence = MakeInfluence(context, religions, factionEntity.EffectId);
                var techs = new List<TechnologyTier>();
                var universalEffects = new List<Effect>();
                var antilegacyEffects = new List<Effect>();
                var universalIncomes = new List<Income>();
                var antilegacyIncomes = new List<Income>();
                var universalInfluence = default(Influence);
                var antilegacyInfluence = default(Influence);
                var universalLocks = new List<BuildingLevel>();
                var universalUnlocks = new List<BuildingLevel>();
                var antilegacyLocks = new List<BuildingLevel>();
                var antilegacyUnlocks = new List<BuildingLevel>();
                foreach (var techEntity in context.TechnologyLevels.Where(x => x.FactionId == factionEntity.Id).OrderBy(x => x.Order).ToList())
                {
                    universalEffects.Add(MakeEffect(context, techEntity.UniversalEffectId));
                    antilegacyEffects.Add(MakeEffect(context, techEntity.AntilegacyEffectId));
                    universalIncomes.AddRange(MakeIncomes(context, techEntity.UniversalEffectId));
                    antilegacyIncomes.AddRange(MakeIncomes(context, techEntity.AntilegacyEffectId));
                    universalInfluence += MakeInfluence(context, religions, techEntity.UniversalEffectId);
                    antilegacyInfluence += MakeInfluence(context, religions, techEntity.AntilegacyEffectId);
                    var universalLocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && !y.Antilegacy && y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var universalUnlocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && !y.Antilegacy && !y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var antilegacyLocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && y.Antilegacy && y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var antilegacyUnlocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && y.Antilegacy && !y.Lock).Select(x => x.BuildingLevelId).ToList();
                    universalLocks.AddRange(buildings.Where(x => universalLocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    universalUnlocks.AddRange(buildings.Where(x => universalUnlocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    antilegacyLocks.AddRange(buildings.Where(x => antilegacyLocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    antilegacyUnlocks.AddRange(buildings.Where(x => antilegacyUnlocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    techs.Add(new TechnologyTier(universalEffects, antilegacyEffects, universalIncomes, antilegacyIncomes, universalInfluence, antilegacyInfluence, universalLocks, universalUnlocks, antilegacyLocks, antilegacyUnlocks));
                }

                var factionBranches = new List<BuildingBranch>();
                foreach (var useEntity in context.BuildingBranchUses.Where(x => x.FactionId == factionEntity.Id))
                {
                    var usedBranches = branches.Where(x => useEntity.BuildingBranchId == x.Key);
                    factionBranches.AddRange(usedBranches.Select(x => x.Value));
                }

                factions.Add(new KeyValuePair<int, Faction>(factionEntity.Id, new Faction(factionEntity.Name, techs, factionBranches, effect, incomes, influence)));
            }

            this.Religions = religions.Select(x => x.Value);
            this.Provinces = provinces.Select(x => x.Value);
            this.Factions = factions.Select(x => x.Value);
        }
    }

    public IEnumerable<Religion> Religions { get; init; }

    public IEnumerable<Province> Provinces { get; init; }

    public IEnumerable<Faction> Factions { get; init; }

    public IEnumerable<Weather> GetWeathers()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Weathers
                .OrderBy(x => x.Order)
                .ToList();

            var models = new List<Weather>();
            foreach (var entity in entities)
            {
                models.Add(WeatherOperations.Create(entity.Name));
            }

            return models;
        }
    }

    public IEnumerable<Season> GetSeasons()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Seasons
                .OrderBy(x => x.Order)
                .ToList();

            var models = new List<Season>();
            foreach (var entity in entities)
            {
                models.Add(SeasonOperations.Create(entity.Name));
            }

            return models;
        }
    }

    private static Effect MakeEffect(DatabaseContext context, int? id)
    {
        Effect effect = default;
        if (id.HasValue)
        {
            var effectEntity = context.Effects.Find(id);
            effect = EffectOperations.Create(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, effectEntity.RegionalSanitation);
        }

        return effect;
    }

    private static IEnumerable<Income> MakeIncomes(DatabaseContext context, int? id)
    {
        IEnumerable<Income> incomes = Enumerable.Empty<Income>();
        if (id.HasValue)
        {
            var bonusEntities = context.Bonuses.Where(x => x.EffectId == id).ToList();
            incomes = bonusEntities.Select(x => IncomeOperations.Create(x.Value, x.Category, x.Type)).ToArray();
        }

        return incomes;
    }

    private static Influence MakeInfluence(DatabaseContext context, List<KeyValuePair<int, Religion>> religions, int? id)
    {
        Influence influence = default;
        if (id.HasValue)
        {
            var influenceEntities = context.Influences.Where(x => x.EffectId == id).ToList();
            influence = influenceEntities.Select(x => new Influence(x.ReligionId.HasValue ? religions.Single(y => y.Key == x.ReligionId).Value : null, x.Value)).Aggregate(default(Influence), (x, y) => x + y);
        }

        return influence;
    }
}