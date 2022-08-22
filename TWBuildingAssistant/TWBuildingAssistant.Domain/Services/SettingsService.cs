namespace TWBuildingAssistant.Domain.Services;

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

    public async Task<EffectSet> GetStateFromSettings(Settings settings)
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var factionEffects = await this.GetFactionwideEffects(context, settings);
            var technologyEffects = await this.GetTechnologyEffect(context, settings);
            var religionEffects = await this.GetReligionEffect(context, settings);
            var provinceEffects = await this.GetProvinceEffect(context, settings);
            var climateEffects = await this.GetClimateEffect(context, settings);
            var difficultyEffects = await this.GetDifficultyEffect(context, settings);
            var taxEffects = await this.GetTaxEffect(context, settings);
            var fertilityDropEffect = EffectOperations.Create(fertility: settings.FertilityDrop);
            var corruptionIncome = IncomeOperations.Create(-settings.CorruptionRate, null, BonusType.Percentage);
            var piracyIncome = IncomeOperations.Create(-settings.PiracyRate, IncomeCategory.MaritimeCommerce, BonusType.Percentage);

            var effects = new[]
            {
                factionEffects.Effect,
                technologyEffects.Effect,
                religionEffects.Effect,
                provinceEffects.Effect,
                climateEffects.Effect,
                difficultyEffects.Effect,
                taxEffects.Effect,
                fertilityDropEffect,
            };

            var incomes = factionEffects.Incomes
                .Concat(technologyEffects.Incomes)
                .Concat(religionEffects.Incomes)
                .Concat(provinceEffects.Incomes)
                .Concat(climateEffects.Incomes)
                .Concat(difficultyEffects.Incomes)
                .Concat(taxEffects.Incomes)
                .Append(corruptionIncome)
                .Append(piracyIncome);

            var influences = factionEffects.Influences
                .Concat(technologyEffects.Influences)
                .Concat(religionEffects.Influences)
                .Concat(provinceEffects.Influences)
                .Concat(climateEffects.Influences)
                .Concat(difficultyEffects.Influences)
                .Concat(taxEffects.Influences);

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
            (var effect, var incomes, var influences) = await this.GetEffect(buildingLevelEntity.EffectId);
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
                var levelEffect = await this.GetEffect(level.EffectId);
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
        var effectId = await context.Factions
            .AsNoTracking()
            .Where(x => x.Id == settings.FactionId)
            .Select(x => x.EffectId)
            .FirstOrDefaultAsync();

        return await this.GetEffect(effectId);
    }

    private async Task<EffectSet> GetTechnologyEffect(DatabaseContext context, Settings settings)
    {
        var universalEffectIds = await context.TechnologyLevels
            .AsNoTracking()
            .Where(x => x.FactionId == settings.FactionId)
            .Where(x => x.Order <= settings.TechnologyTier)
            .Select(x => x.UniversalEffectId)
            .ToListAsync();

        var effectTasks = universalEffectIds.Select(x => this.GetEffect(x));
        var effects = (await Task.WhenAll(effectTasks)).AsEnumerable();

        if (settings.UseAntilegacyTechnologies)
        {
            var antilegacyEffectIds = await context.TechnologyLevels
                .AsNoTracking()
                .Where(x => x.FactionId == settings.FactionId)
                .Where(x => x.Order <= settings.TechnologyTier)
                .Select(x => x.AntilegacyEffectId)
                .ToListAsync();

            effectTasks = antilegacyEffectIds.Select(x => this.GetEffect(x));
            effects = effects.Concat(await Task.WhenAll(effectTasks));
        }

        return new EffectSet(EffectOperations.Collect(effects.Select(x => x.Effect)), effects.SelectMany(x => x.Incomes).ToImmutableArray(), effects.SelectMany(x => x.Influences).ToImmutableArray());
    }

    private async Task<EffectSet> GetReligionEffect(DatabaseContext context, Settings settings)
    {
        var effectId = await context.Religions
            .AsNoTracking()
            .Where(x => x.Id == settings.ReligionId)
            .Select(x => x.EffectId)
            .FirstOrDefaultAsync();

        return await this.GetEffect(effectId);
    }

    private async Task<EffectSet> GetProvinceEffect(DatabaseContext context, Settings settings)
    {
        var effectId = await context.Provinces
            .AsNoTracking()
            .Where(x => x.Id == settings.ProvinceId)
            .Select(x => x.EffectId)
            .FirstOrDefaultAsync();

        return await this.GetEffect(effectId);
    }

    private async Task<EffectSet> GetClimateEffect(DatabaseContext context, Settings settings)
    {
        var effectId = await context.Provinces
            .AsNoTracking()
            .Where(x => x.Id == settings.ProvinceId)
            .SelectMany(x => x.Climate.Effects)
            .Where(x => x.WeatherId == settings.WeatherId && x.SeasonId == settings.SeasonId)
            .Select(x => x.EffectId)
            .FirstOrDefaultAsync();

        return await this.GetEffect(effectId);
    }

    private async Task<EffectSet> GetDifficultyEffect(DatabaseContext context, Settings settings)
    {
        var effectId = await context.Difficulties
            .AsNoTracking()
            .Where(x => x.Id == settings.DifficultyId)
            .Select(x => x.EffectId)
            .FirstOrDefaultAsync();

        return await this.GetEffect(effectId);
    }

    private async Task<EffectSet> GetTaxEffect(DatabaseContext context, Settings settings)
    {
        var effectId = await context.Taxes
            .AsNoTracking()
            .Where(x => x.Id == settings.DifficultyId)
            .Select(x => x.EffectId)
            .FirstOrDefaultAsync();

        return await this.GetEffect(effectId);
    }

    private async Task<EffectSet> GetEffect(int? effectId)
    {
        var effect = default(Effect);
        var incomes = Enumerable.Empty<Income>();
        var influences = Enumerable.Empty<Influence>();
        if (effectId is not null)
        {
            using (var context = this.contextFactory.CreateDbContext())
            {
                var effectEntity = await context.Effects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == effectId);

                var incomeEntities = await context.Bonuses
                    .AsNoTracking()
                    .Where(x => x.EffectId == effectId)
                    .ToListAsync();

                var influenceEntities = await context.Influences
                    .AsNoTracking()
                    .Where(x => x.EffectId == effectId)
                    .ToListAsync();

                if (effectEntity is not null)
                {
                    effect = EffectOperations.Create(effectEntity.PublicOrder, effectEntity.RegularFood, effectEntity.FertilityDependentFood, effectEntity.ProvincialSanitation, effectEntity.ResearchRate, effectEntity.Growth, effectEntity.Fertility, effectEntity.ReligiousOsmosis, effectEntity.RegionalSanitation);
                }

                incomes = incomeEntities.Select(x => IncomeOperations.Create(x.Value, x.Category, x.Type));
                influences = influenceEntities.Select(x => InfluenceOperations.Create(x.ReligionId, x.Value));
            }
        }

        return new EffectSet(effect, incomes.ToImmutableArray(), influences.ToImmutableArray());
    }
}