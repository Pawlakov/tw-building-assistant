namespace TWBuildingAssistant.Domain.Services;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using EnumsNET;
using Microsoft.EntityFrameworkCore;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Data.Sqlite;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.StateModels;

public class SettingsService
    : ISettingsService
{
    private readonly DatabaseContextFactory contextFactory;

    public SettingsService(DatabaseContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<IEnumerable<NamedId>> GetFactionOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Factions
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<IEnumerable<NamedId>> GetProvinceOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Provinces
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<IEnumerable<NamedId>> GetReligionOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Religions
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<IEnumerable<NamedId>> GetSeasonOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Seasons
                .OrderBy(x => x.Order)
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<IEnumerable<NamedId>> GetWeatherOptions()
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var entities = context.Weathers
                .OrderBy(x => x.Order)
                .ToList();

            var models = new List<NamedId>();
            foreach (var entity in entities)
            {
                models.Add(new NamedId(entity.Id, entity.Name));
            }

            return models;
        }
    }

    public async Task<EffectSet> GetStateFromSettings(Settings settings)
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var factionEffects = await this.GetFactionwideEffects(context, settings);
            var technologyEffects = await this.GetTechnologyEffect(context, settings);
            var religionEffects = await this.GetReligionEffect(context, settings);
            var provinceEffects = await this.GetProvinceEffect(context, settings);
            var climateEffects = await this.GetClimateEffect(context, settings);
            var fertilityDropEffect = EffectOperations.Create(fertility: settings.FertilityDrop);
            var corruptionIncome = IncomeOperations.Create(-settings.CorruptionRate, null, BonusType.Percentage);

            var effects = new[]
            {
                factionEffects.Effect,
                technologyEffects.Effect,
                religionEffects.Effect,
                provinceEffects.Effect,
                climateEffects.Effect,
                fertilityDropEffect,
            };

            var incomes = factionEffects.Incomes
                .Concat(technologyEffects.Incomes)
                .Concat(religionEffects.Incomes)
                .Concat(provinceEffects.Incomes)
                .Concat(climateEffects.Incomes)
                .Append(corruptionIncome);

            var influences = factionEffects.Influences
                .Concat(technologyEffects.Influences)
                .Concat(religionEffects.Influences)
                .Concat(provinceEffects.Influences)
                .Concat(climateEffects.Influences);

            return new EffectSet(EffectOperations.Collect(effects), incomes.ToImmutableArray(), influences.ToImmutableArray());
        }
    }

    public async Task<ImmutableArray<BuildingLibraryEntry>> GetBuildingLibrary(Settings settings)
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var slotTypes = Enums.GetValues<SlotType>();
            var regionTypes = Enums.GetValues<RegionType>();
            var resourceIdsInProvince = await context.Regions
                .AsNoTracking()
                .Where(x => x.ProvinceId == settings.ProvinceId)
                .Select(x => x.ResourceId)
                .Distinct()
                .ToListAsync();

            var results = new List<BuildingLibraryEntry>();
            foreach (var descriptor in slotTypes.SelectMany(x => regionTypes.SelectMany(y => resourceIdsInProvince.Select(z => new SlotDescriptor(x, y, z)))))
            {
                results.Add(await this.GetBuildingLibraryEntry(context, settings, descriptor));
            }

            return results.ToImmutableArray();
        }
    }

    private async Task<BuildingLibraryEntry> GetBuildingLibraryEntry(DatabaseContext context, Settings settings, SlotDescriptor descriptor)
    {
        var usedBranchEntitites = await context.BuildingBranchUses
            .AsNoTracking()
            .Where(x => x.FactionId == settings.FactionId)
            .Select(x => x.BuildingBranch)
            .Where(x =>
                x.SlotType == descriptor.SlotType &&
                (x.RegionType == null || x.RegionType == descriptor.RegionType) &&
                (x.ReligionId == null || x.ReligionId == settings.ReligionId) &&
                (x.ResourceId == null || x.ResourceId == descriptor.ResourceId))
            .ToListAsync();

        var unlockedLevelIds = await this.GetUnlockedBuildingLevelIds(context, settings);
        var unlockedLevelEntitites = await context.BuildingLevels
            .AsNoTracking()
            .Include(x => x.Effect.Bonuses)
            .Include(x => x.Effect.Influences)
            .Where(x => unlockedLevelIds.Contains(x.Id))
            .ToListAsync();

        var strainPairs = new List<(Data.Sqlite.Entities.BuildingBranch Branch, IEnumerable<Data.Sqlite.Entities.BuildingLevel> Levels)>();
        foreach (var branchEntity in usedBranchEntitites)
        {
            var strains = new List<IEnumerable<Data.Sqlite.Entities.BuildingLevel>>();
            void RecursiveBranchTraversing(IEnumerable<Data.Sqlite.Entities.BuildingLevel> ancestors, Data.Sqlite.Entities.BuildingLevel current)
            {
                var branchLevels = ancestors.Append(current);
                var children = unlockedLevelEntitites.Where(x => x.ParentBuildingLevelId == current.Id);
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

            RecursiveBranchTraversing(Enumerable.Empty<Data.Sqlite.Entities.BuildingLevel>(), unlockedLevelEntitites.Single(x => x.Id == branchEntity.RootBuildingLevelId));
            if (branchEntity.AllowParallel)
            {
                foreach (var branchLevels in strains)
                {
                    strainPairs.Add((branchEntity, branchLevels));
                }
            }
            else
            {
                var branchLevels = strains.SelectMany(x => x).Distinct();
                strainPairs.Add((branchEntity, branchLevels));
            }
        }

        var buildings = new List<(BuildingLevel Level, int? ParentId)>();
        foreach (var buildingLevelEntity in unlockedLevelEntitites)
        {
            (var effect, var incomes, var influences) = this.MakeEffect(buildingLevelEntity.Effect);
            buildings.Add((new BuildingLevel(buildingLevelEntity.Id, buildingLevelEntity.Name, effect, incomes, influences), buildingLevelEntity.ParentBuildingLevelId));
        }

        var finalDictionary = new List<BuildingBranch>();
        if (descriptor.SlotType == SlotType.General)
        {
            finalDictionary.Add(BuildingBranchOperations.Empty);
        }

        foreach (var strain in strainPairs)
        {
            var levelsOther = new List<BuildingLevel>();
            foreach (var level in strain.Levels)
            {
                var levelEffect = this.MakeEffect(level.Effect);
                levelsOther.Add(new BuildingLevel(level.Id, level.Name, levelEffect.Effect, levelEffect.Incomes, levelEffect.Influences));
            }

            if (levelsOther.Any())
            {
                finalDictionary.Add(new BuildingBranch(strain.Branch.Id, strain.Branch.Name, strain.Branch.Interesting, levelsOther.ToImmutableArray()));
            }
        }

        return new BuildingLibraryEntry(descriptor, finalDictionary.ToImmutableArray());
    }

    private async Task<List<int>> GetUnlockedBuildingLevelIds(DatabaseContext context, Settings settings)
    {
        var locksEntities = await context.TechnologyLevels
            .AsNoTracking()
            .Where(x => x.FactionId == settings.FactionId && x.Order <= settings.TechnologyTier)
            .SelectMany(x => x.BuildingLevelLocks)
            .Where(x => settings.UseAntilegacyTechnologies || !x.Antilegacy)
            .ToListAsync();

        var lockedIds = new List<int>();
        var unlockedIds = new List<int>();
        foreach (var lockEntity in locksEntities)
        {
            if (lockEntity.Lock)
            {
                lockedIds.Add(lockEntity.BuildingLevelId);
            }
            else
            {
                unlockedIds.Add(lockEntity.BuildingLevelId);
            }
        }

        return unlockedIds.Except(lockedIds).ToList();
    }

    private async Task<EffectSet> GetFactionwideEffects(DatabaseContext context, Settings settings)
    {
        var effectEntity = await context.Factions
            .AsNoTracking()
            .Include(x => x.Effect.Bonuses)
            .Include(x => x.Effect.Influences)
            .Where(x => x.Id == settings.FactionId)
            .Select(x => x.Effect)
            .FirstOrDefaultAsync();

        return this.MakeEffect(effectEntity);
    }

    private async Task<EffectSet> GetTechnologyEffect(DatabaseContext context, Settings settings)
    {
        var universalEffectEntities = await context.TechnologyLevels
            .AsNoTracking()
            .Include(x => x.UniversalEffect.Bonuses)
            .Include(x => x.UniversalEffect.Influences)
            .Where(x => x.FactionId == settings.FactionId)
            .Where(x => x.Order <= settings.TechnologyTier)
            .Select(x => x.UniversalEffect)
            .ToListAsync();

        var effects = universalEffectEntities.Select(x => this.MakeEffect(x));

        if (settings.UseAntilegacyTechnologies)
        {
            var antilegacyEffectEntities = await context.TechnologyLevels
                .AsNoTracking()
                .Include(x => x.AntilegacyEffect.Bonuses)
                .Include(x => x.AntilegacyEffect.Influences)
                .Where(x => x.FactionId == settings.FactionId)
                .Where(x => x.Order <= settings.TechnologyTier)
                .Select(x => x.AntilegacyEffect)
                .Include(x => x.Bonuses)
                .Include(x => x.Influences)
                .ToListAsync();

            effects = effects.Concat(antilegacyEffectEntities.Select(x => this.MakeEffect(x)));
        }

        return new EffectSet(EffectOperations.Collect(effects.Select(x => x.Effect)), effects.SelectMany(x => x.Incomes).ToImmutableArray(), effects.SelectMany(x => x.Influences).ToImmutableArray());
    }

    private async Task<EffectSet> GetReligionEffect(DatabaseContext context, Settings settings)
    {
        var effectEntity = await context.Religions
            .AsNoTracking()
            .Include(x => x.Effect.Bonuses)
            .Include(x => x.Effect.Influences)
            .Where(x => x.Id == settings.ReligionId)
            .Select(x => x.Effect)
            .FirstOrDefaultAsync();

        return this.MakeEffect(effectEntity);
    }

    private async Task<EffectSet> GetProvinceEffect(DatabaseContext context, Settings settings)
    {
        var effectEntity = await context.Provinces
            .AsNoTracking()
            .Include(x => x.Effect.Bonuses)
            .Include(x => x.Effect.Influences)
            .Where(x => x.Id == settings.ProvinceId)
            .Select(x => x.Effect)
            .FirstOrDefaultAsync();

        return this.MakeEffect(effectEntity);
    }

    private async Task<EffectSet> GetClimateEffect(DatabaseContext context, Settings settings)
    {
        var effectEntity = await context.Provinces
            .AsNoTracking()
            .Include(x => x.Climate.Effects)
            .ThenInclude(x => x.Effect.Influences)
            .Include(x => x.Climate.Effects)
            .ThenInclude(x => x.Effect.Influences)
            .Where(x => x.Id == settings.ProvinceId)
            .SelectMany(x => x.Climate.Effects)
            .Where(x => x.WeatherId == settings.WeatherId && x.SeasonId == settings.SeasonId)
            .Select(x => x.Effect)
            .FirstOrDefaultAsync();

        return this.MakeEffect(effectEntity);
    }

    private EffectSet MakeEffect(Data.Sqlite.Entities.Effect? effectEntity)
    {
        Effect effect = default;
        IEnumerable<Income> incomes = Enumerable.Empty<Income>();
        IEnumerable<Influence> influences = Enumerable.Empty<Influence>();
        if (effectEntity is not null)
        {
            effect = EffectOperations.Create(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, effectEntity.RegionalSanitation);
            if (effectEntity.Bonuses is not null)
            {
                incomes = effectEntity.Bonuses.Select(x => IncomeOperations.Create(x.Value, x.Category, x.Type));
            }

            if (effectEntity.Influences is not null)
            {
                influences = effectEntity.Influences.Select(x => InfluenceOperations.Create(x.ReligionId, x.Value));
            }
        }

        return new EffectSet(effect, incomes.ToImmutableArray(), influences.ToImmutableArray());
    }
}