namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Domain.Exceptions;

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
                resources.Add(new KeyValuePair<int, Resource>(resourceEntity.Id, new Resource(resourceEntity.Name)));
            }

            var weathers = new List<KeyValuePair<int, Weather>>();
            foreach (var weatherEntity in context.Weathers.OrderBy(x => x.Order).ToList())
            {
                weathers.Add(new KeyValuePair<int, Weather>(weatherEntity.Id, new Weather(weatherEntity.Name)));
            }

            var seasons = new List<KeyValuePair<int, Season>>();
            foreach (var seasonEntity in context.Seasons.OrderBy(x => x.Order).ToList())
            {
                seasons.Add(new KeyValuePair<int, Season>(seasonEntity.Id, new Season(seasonEntity.Name)));
            }

            var religions = new List<KeyValuePair<int, Religion>>();
            foreach (var religionEntity in context.Religions.ToList())
            {
                Effect effect = default;
                if (religionEntity.EffectId.HasValue)
                {
                    var effectEntity = context.Effects.Find(religionEntity.EffectId.Value);
                    var bonusEntities = context.Bonuses.Where(x => x.EffectId == effectEntity.Id).ToList();
                    var influenceEntities = context.Influences.Where(x => x.EffectId == effectEntity.Id).ToList();
                    if (influenceEntities.Any(x => x.ReligionId.HasValue))
                    {
                        throw new DomainRuleViolationException("Influence of a religion with a set religion id.");
                    }

                    var influence = influenceEntities.Select(x => new Influence(null, x.Value)).Aggregate(default(Influence), (x, y) => x + y);
                    var bonus = bonusEntities.Select(x => new Income(x.Value, x.Category, x.Type)).Aggregate(default(Income), (x, y) => x + y);
                    effect = new Effect(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, 0, bonus, influence);
                }

                religions.Add(new KeyValuePair<int, Religion>(religionEntity.Id, new Religion(religionEntity.Name, effect)));
            }

            var climates = new List<KeyValuePair<int, Climate>>();
            foreach (var climateEntity in context.Climates.ToList())
            {
                var weatherEffects = new Dictionary<Season, IDictionary<Weather, Effect>>();
                foreach (var season in seasons)
                {
                    var entry = new Dictionary<Weather, Effect>();
                    foreach (var weather in weathers)
                    {
                        var weatherEffectEntity = context.WeatherEffects.SingleOrDefault(x => x.SeasonId == season.Key && x.ClimateId == climateEntity.Id && x.WeatherId == weather.Key);
                        var effect = MakeEffect(context, religions, weatherEffectEntity?.EffectId);
                        entry.Add(weather.Value, effect);
                    }

                    weatherEffects.Add(season.Value, entry);
                }

                climates.Add(new KeyValuePair<int, Climate>(climateEntity.Id, new Climate(weatherEffects)));
            }

            var provinces = new List<KeyValuePair<int, Province>>();
            foreach (var provinceEntity in context.Provinces.ToList())
            {
                var effect = MakeEffect(context, religions, provinceEntity.EffectId);
                var regions = new List<Region>();
                foreach (var regionEntity in context.Regions.Where(x => x.ProvinceId == provinceEntity.Id).ToList())
                {
                    regions.Add(new Region(regionEntity.Name, regionEntity.RegionType, regionEntity.IsCoastal, regionEntity.ResourceId.HasValue ? resources.Single(x => x.Key == regionEntity.ResourceId).Value : null, regionEntity.SlotsCountOffset == -1));
                }

                provinces.Add(new KeyValuePair<int, Province>(provinceEntity.Id, new Province(provinceEntity.Name, regions, climates.Single(x => x.Key == provinceEntity.ClimateId).Value, effect)));
            }

            var buildings = new List<KeyValuePair<int, Tuple<BuildingLevel, int?>>>();
            foreach (var buildingLevelEntity in context.BuildingLevels)
            {
                var effect = MakeEffect(context, religions, buildingLevelEntity.EffectId);
                buildings.Add(new KeyValuePair<int, Tuple<BuildingLevel, int?>>(buildingLevelEntity.Id, Tuple.Create(new BuildingLevel(buildingLevelEntity.Name, effect), buildingLevelEntity.ParentBuildingLevelId)));
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
                var effect = MakeEffect(context, religions, factionEntity.EffectId);
                var techs = new List<TechnologyTier>();
                var universalEffect = default(Effect);
                var antilegacyEffect = default(Effect);
                var universalLocks = new List<BuildingLevel>();
                var universalUnlocks = new List<BuildingLevel>();
                var antilegacyLocks = new List<BuildingLevel>();
                var antilegacyUnlocks = new List<BuildingLevel>();
                foreach (var techEntity in context.TechnologyLevels.Where(x => x.FactionId == factionEntity.Id).OrderBy(x => x.Order).ToList())
                {
                    universalEffect += MakeEffect(context, religions, techEntity.UniversalEffectId);
                    antilegacyEffect += MakeEffect(context, religions, techEntity.AntilegacyEffectId);
                    var universalLocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && !y.Antilegacy && y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var universalUnlocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && !y.Antilegacy && !y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var antilegacyLocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && y.Antilegacy && y.Lock).Select(x => x.BuildingLevelId).ToList();
                    var antilegacyUnlocksIds = context.BuildingLevelLocks.Where(y => y.TechnologyLevelId == techEntity.Id && y.Antilegacy && !y.Lock).Select(x => x.BuildingLevelId).ToList();
                    universalLocks.AddRange(buildings.Where(x => universalLocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    universalUnlocks.AddRange(buildings.Where(x => universalUnlocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    antilegacyLocks.AddRange(buildings.Where(x => antilegacyLocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    antilegacyUnlocks.AddRange(buildings.Where(x => antilegacyUnlocksIds.Contains(x.Key)).Select(x => x.Value.Item1));
                    techs.Add(new TechnologyTier(universalEffect, antilegacyEffect, universalLocks, universalUnlocks, antilegacyLocks, antilegacyUnlocks));
                }

                var factionBranches = new List<BuildingBranch>();
                foreach (var useEntity in context.BuildingBranchUses.Where(x => x.FactionId == factionEntity.Id))
                {
                    var usedBranches = branches.Where(x => useEntity.BuildingBranchId == x.Key);
                    factionBranches.AddRange(usedBranches.Select(x => x.Value));
                }

                factions.Add(new KeyValuePair<int, Faction>(factionEntity.Id, new Faction(factionEntity.Name, techs, factionBranches, effect)));
            }

            Religions = religions.Select(x => x.Value);
            Provinces = provinces.Select(x => x.Value);
            Factions = factions.Select(x => x.Value);
            Weathers = weathers.Select(x => x.Value);
            Seasons = seasons.Select(x => x.Value);
        }
    }

    public IEnumerable<Religion> Religions { get; init; }

    public IEnumerable<Province> Provinces { get; init; }

    public IEnumerable<Faction> Factions { get; init; }

    public IEnumerable<Weather> Weathers { get; init; }

    public IEnumerable<Season> Seasons { get; init; }

    private static Effect MakeEffect(DatabaseContext context, List<KeyValuePair<int, Religion>> religions, int? id)
    {
        Effect effect = default;
        if (id.HasValue)
        {
            var effectEntity = context.Effects.Find(id);
            var bonusEntities = context.Bonuses.Where(x => x.EffectId == effectEntity.Id).ToList();
            var influenceEntities = context.Influences.Where(x => x.EffectId == effectEntity.Id).ToList();

            var influence = influenceEntities.Select(x => new Influence(x.ReligionId.HasValue ? religions.Single(y => y.Key == x.ReligionId).Value : null, x.Value)).Aggregate(default(Influence), (x, y) => x + y);
            var bonus = bonusEntities.Select(x => new Income(x.Value, x.Category, x.Type)).Aggregate(default(Income), (x, y) => x + y);
            effect = new Effect(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, effectEntity.RegionalSanitation, bonus, influence);
        }

        return effect;
    }
}