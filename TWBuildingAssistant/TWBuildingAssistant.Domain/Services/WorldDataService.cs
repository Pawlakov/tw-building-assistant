namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;
using TWBuildingAssistant.Domain.OldModels;

public class WorldDataService
    : IWorldDataService
{
    private readonly DatabaseContextFactory contextFactory;

    public WorldDataService(DatabaseContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public IEnumerable<Faction> GetFactions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
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

            var factions = new List<Faction>();
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

                factions.Add(new Faction(factionEntity.Id, factionEntity.Name, techs, factionBranches, effect, incomes, influences));
            }

            return factions;
        }
    }

    public IEnumerable<Province> GetProvinces()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Provinces
                .AsNoTracking()
                .Include(x => x.Regions)
                .ToList();

            var models = new List<Province>();
            foreach (var entity in entities)
            {
                var effect = MakeEffect(context, entity.EffectId);
                var incomes = MakeIncomes(context, entity.EffectId);
                var influence = MakeInfluences(context, entity.EffectId);
                var regions = new List<Region>();
                foreach (var regionEntity in entity.Regions)
                {
                    regions.Add(new Region(regionEntity.Name, regionEntity.RegionType, regionEntity.IsCoastal, regionEntity.ResourceId, regionEntity.SlotsCountOffset == -1));
                }

                models.Add(new Province(entity.Id, entity.Name, regions, entity.ClimateId, effect, incomes, influence));
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