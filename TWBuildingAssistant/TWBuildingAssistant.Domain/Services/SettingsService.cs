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

    public async Task<ImmutableArray<BuildingLibraryEntry>> GetBuildingLibrary(Data.FSharp.Models.Settings settings)
    {
        using (var context = this.contextFactory.CreateDbContext())
        {
            var slotTypes = new[] { Data.FSharp.Models.SlotType.Main, Data.FSharp.Models.SlotType.Coastal, Data.FSharp.Models.SlotType.General, };
            var regionTypes = new[] { Data.FSharp.Models.RegionType.City, Data.FSharp.Models.RegionType.Town, };
            var resourceIdsInProvince = await context.Regions
                .AsNoTracking()
                .Where(x => x.ProvinceId == settings.ProvinceId)
                .Select(x => x.ResourceId)
                .Distinct()
                .ToListAsync();

            var results = new List<BuildingLibraryEntry>();
            foreach (var descriptor in slotTypes.SelectMany(x => regionTypes.SelectMany(y => resourceIdsInProvince.Select(z => new Data.FSharp.Models.SlotDescriptor(x, y, z)))))
            {
                results.Add(await this.GetBuildingLibraryEntry(context, settings, descriptor));
            }

            return results.ToImmutableArray();
        }
    }

    private async Task<BuildingLibraryEntry> GetBuildingLibraryEntry(DatabaseContext context, Data.FSharp.Models.Settings settings, Data.FSharp.Models.SlotDescriptor descriptor)
    {
        var usedBranchEntitites = await context.BuildingBranchUses
            .AsNoTracking()
            .Where(x => x.FactionId == settings.FactionId)
            .Select(x => x.BuildingBranch)
            .Where(x =>
                (int)x.SlotType == descriptor.SlotType.Tag &&
                (x.RegionType == null || (int)x.RegionType == descriptor.RegionType.Tag) &&
                (x.ReligionId == null || x.ReligionId == settings.ReligionId) &&
                (x.ResourceId == null || (Microsoft.FSharp.Core.OptionModule.IsSome(descriptor.ResourceId) && x.ResourceId.Value == descriptor.ResourceId.Value)))
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

        var finalDictionary = new List<BuildingBranch>();
        if (descriptor.SlotType == Data.FSharp.Models.SlotType.General)
        {
            finalDictionary.Add(BuildingBranchOperations.Empty);
        }

        foreach (var strain in strainPairs)
        {
            var levelsOther = new List<BuildingLevel>();
            foreach (var level in strain.Levels)
            {
                var levelEffect = Data.FSharp.Library.getEffectOption(level.EffectId);
                levelsOther.Add(new BuildingLevel(level.Id, level.Name, levelEffect.Effect, levelEffect.Incomes, levelEffect.Influences));
            }

            if (levelsOther.Any())
            {
                finalDictionary.Add(new BuildingBranch(strain.Branch.Id, strain.Branch.Name, strain.Branch.Interesting, levelsOther.ToImmutableArray()));
            }
        }

        return new BuildingLibraryEntry(descriptor, finalDictionary.ToImmutableArray());
    }

    private async Task<List<int>> GetUnlockedBuildingLevelIds(DatabaseContext context, Data.FSharp.Models.Settings settings)
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
}