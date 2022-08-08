namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
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
            var provinces = new List<KeyValuePair<int, Province>>();
            foreach (var provinceEntity in context.Provinces.ToList())
            {
                var effect = MakeEffect(context, provinceEntity.EffectId);
                var incomes = MakeIncomes(context, provinceEntity.EffectId);
                var influence = MakeInfluences(context, provinceEntity.EffectId);
                var regions = new List<Region>();
                foreach (var regionEntity in context.Regions.Where(x => x.ProvinceId == provinceEntity.Id).ToList())
                {
                    regions.Add(new Region(regionEntity.Name, regionEntity.RegionType, regionEntity.IsCoastal, regionEntity.ResourceId, regionEntity.SlotsCountOffset == -1));
                }

                provinces.Add(new KeyValuePair<int, Province>(provinceEntity.Id, new Province(provinceEntity.Name, regions, provinceEntity.ClimateId, effect, incomes, influence)));
            }

            var buildings = new List<KeyValuePair<int, Tuple<BuildingLevel, int?>>>();
            foreach (var buildingLevelEntity in context.BuildingLevels)
            {
                var effect = MakeEffect(context, buildingLevelEntity.EffectId);
                var incomes = MakeIncomes(context, buildingLevelEntity.EffectId);
                var influence = MakeInfluences(context, buildingLevelEntity.EffectId);
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
                        branches.Add(new KeyValuePair<int, BuildingBranch>(branchEntity.Id, new BuildingBranch(branchEntity.SlotType, branchEntity.RegionType, branchEntity.ResourceId, branchEntity.ReligionId, branchLevels)));
                    }
                }
                else
                {
                    var branchLevels = strains.SelectMany(x => x).Distinct();
                    branches.Add(new KeyValuePair<int, BuildingBranch>(branchEntity.Id, new BuildingBranch(branchEntity.SlotType, branchEntity.RegionType, branchEntity.ResourceId, branchEntity.ReligionId, branchLevels)));
                }
            }

            var factions = new List<KeyValuePair<int, Faction>>();
            foreach (var factionEntity in context.Factions.ToList())
            {
                var effect = MakeEffect(context, factionEntity.EffectId);
                var incomes = MakeIncomes(context, factionEntity.EffectId);
                var influences = MakeInfluences(context, factionEntity.EffectId);
                var techs = new List<TechnologyTier>();
                var universalEffects = new List<Effect>();
                var antilegacyEffects = new List<Effect>();
                var universalIncomes = new List<Income>();
                var antilegacyIncomes = new List<Income>();
                var universalInfluences = new List<Influence>();
                var antilegacyInfluences = new List<Influence>();
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
                    universalInfluences.AddRange(MakeInfluences(context, techEntity.UniversalEffectId));
                    antilegacyInfluences.AddRange(MakeInfluences(context, techEntity.AntilegacyEffectId));
                    var universalLocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && !y.Antilegacy && y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var universalUnlocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && !y.Antilegacy && !y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var antilegacyLocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && y.Antilegacy && y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var antilegacyUnlocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && y.Antilegacy && !y.Lock).Select(x => x.BuildingLevelId).ToList();
                    universalLocks.AddRange(buildings.Where(x => universalLocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    universalUnlocks.AddRange(buildings.Where(x => universalUnlocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    antilegacyLocks.AddRange(buildings.Where(x => antilegacyLocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    antilegacyUnlocks.AddRange(buildings.Where(x => antilegacyUnlocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    techs.Add(new TechnologyTier(universalEffects, antilegacyEffects, universalIncomes, antilegacyIncomes, universalInfluences, antilegacyInfluences, universalLocks, universalUnlocks, antilegacyLocks, antilegacyUnlocks));
                }

                var factionBranches = new List<BuildingBranch>();
                foreach (var useEntity in context.BuildingBranchUses.Where(x => x.FactionId == factionEntity.Id))
                {
                    var usedBranches = branches.Where(x => useEntity.BuildingBranchId == x.Key);
                    factionBranches.AddRange(usedBranches.Select(x => x.Value));
                }

                factions.Add(new KeyValuePair<int, Faction>(factionEntity.Id, new Faction(factionEntity.Name, techs, factionBranches, effect, incomes, influences)));
            }

            this.Provinces = provinces.Select(x => x.Value);
            this.Factions = factions.Select(x => x.Value);
        }
    }

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
                models.Add(WeatherOperations.Create(entity.Id, entity.Name));
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
                models.Add(SeasonOperations.Create(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public IEnumerable<Climate> GetClimates()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Climates
                .ToList();

            var models = new List<Climate>();
            foreach (var entity in entities)
            {
                var weatherEffects = new List<(int, IEnumerable<(int, Effect, IEnumerable<Income>)>)>();
                var seasonIds = context.WeatherEffects.Where(x => x.ClimateId == entity.Id).Select(x => x.SeasonId).ToList().Distinct();
                var weatherIds = context.WeatherEffects.Where(x => x.ClimateId == entity.Id).Select(x => x.WeatherId).ToList().Distinct();
                foreach (var seasonId in seasonIds)
                {
                    var weatherEntry = new List<(int, Effect, IEnumerable<Income>)>();
                    foreach (var weatherId in weatherIds)
                    {
                        var weatherEffectEntity = context.WeatherEffects.SingleOrDefault(x => x.SeasonId == seasonId && x.ClimateId == entity.Id && x.WeatherId == weatherId);
                        var effect = MakeEffect(context, weatherEffectEntity?.EffectId);
                        var incomes = MakeIncomes(context, weatherEffectEntity?.EffectId);
                        weatherEntry.Add((weatherId, effect, incomes));
                    }

                    weatherEffects.Add((seasonId, weatherEntry));
                }

                models.Add(ClimateOperations.Create(entity.Id, entity.Name, weatherEffects));
            }

            return models;
        }
    }

    public IEnumerable<Religion> GetReligions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Religions
                .ToList();

            var models = new List<Religion>();
            foreach (var entity in entities)
            {
                var effect = MakeEffect(context, entity.EffectId);
                var incomes = MakeIncomes(context, entity.EffectId);
                var influenceEntities = context.Influences.Where(x => x.EffectId == entity.EffectId).ToList();
                if (influenceEntities.Any(x => x.ReligionId.HasValue))
                {
                    throw new DomainRuleViolationException("Influence of a religion with a set religion id.");
                }

                var influence = influenceEntities.Sum(x => x.Value);

                models.Add(ReligionOperations.Create(entity.Id, entity.Name, effect, incomes, influence));
            }

            return models;
        }
    }

    public IEnumerable<Resource> GetResources()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Resources.ToList();

            var models = new List<Resource>();
            foreach (var entity in entities)
            {
                models.Add(ResourceOperations.Create(entity.Name));
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

    private static IEnumerable<Influence> MakeInfluences(DatabaseContext context, int? id)
    {
        IEnumerable<Influence> influences = Enumerable.Empty<Influence>();
        if (id.HasValue)
        {
            var influenceEntities = context.Influences.Where(x => x.EffectId == id).ToList();
            influences = influenceEntities.Select(x => InfluenceOperations.Create(x.ReligionId, x.Value)).ToArray();
        }

        return influences;
    }
}