namespace TWBuildingAssistant.Domain.OldModels;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using EnumsNET;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;
using TWBuildingAssistant.Domain.StateModels;

public class Faction
{
    private readonly Effect baseFactionwideEffect;
    private readonly Income[] baseFactionwideIncomes;
    private readonly Influence[] baseFactionwideInfluences;

    private readonly TechnologyTier[] technologyTiers;

    private readonly BuildingBranch[] buildingBranches;

    private Dictionary<SlotType, Dictionary<RegionType, List<KeyValuePair<int?, List<KeyValuePair<BuildingBranch, BuildingLevel>>>>>> buildings;

    public Faction(int id, string name, IEnumerable<TechnologyTier> technologyTiers, IEnumerable<BuildingBranch> buildingBranches, Effect baseFactionwideEffect, IEnumerable<Income> baseFactionwideIncomes, IEnumerable<Influence> baseFactionwideInfluences)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainRuleViolationException("Faction without name.");
        }

        if (technologyTiers == null)
        {
            throw new DomainRuleViolationException("Missing tech tiers.");
        }

        if (technologyTiers.Count() != 5)
        {
            throw new DomainRuleViolationException("Invalid tech tiers count.");
        }

        this.Id = id;
        this.Name = name;
        this.baseFactionwideEffect = baseFactionwideEffect;
        this.baseFactionwideIncomes = baseFactionwideIncomes.ToArray();
        this.baseFactionwideInfluences = baseFactionwideInfluences.ToArray();
        this.technologyTiers = technologyTiers.ToArray();
        this.buildingBranches = buildingBranches.ToArray();
    }

    public int Id { get; set; }

    public string Name { get; }

    public IEnumerable<Effect> GetFactionwideEffects(in FactionSettings settings, in Religion religion)
    {
        return
            this.technologyTiers[settings.TechnologyTier].UniversalEffects
                .Concat(settings.UseAntilegacyTechnologies ? this.technologyTiers[settings.TechnologyTier].AntilegacyEffects : Enumerable.Empty<Effect>())
                .Append(this.baseFactionwideEffect)
                .Append(religion.EffectWhenState)
                .Append(EffectOperations.Create(fertility: settings.FertilityDrop));
    }

    public IEnumerable<Income> GetFactionwideIncomes(in FactionSettings settings, in Religion religion)
    {
        return
            new[]
            {
                this.baseFactionwideIncomes,
                religion.IncomesWhenState,
                this.technologyTiers[settings.TechnologyTier].UniversalIncomes,
                (settings.UseAntilegacyTechnologies ? this.technologyTiers[settings.TechnologyTier].AntilegacyIncomes : Enumerable.Empty<Income>()),
            }.SelectMany(x => x);
    }

    public IEnumerable<Influence> GetFactionwideInfluence(in FactionSettings settings, in Religion religion)
    {
        return
            new[]
            {
                this.baseFactionwideInfluences,
                this.technologyTiers[settings.TechnologyTier].UniversalInfluences,
                (settings.UseAntilegacyTechnologies ? this.technologyTiers[settings.TechnologyTier].AntilegacyInfluences : Enumerable.Empty<Influence>()),
            }.SelectMany(x => x)
            .Append(InfluenceOperations.Create(null, religion.StateInfluenceWhenState));
    }

    public IEnumerable<BuildingLevel> GetBuildingLevelsForSlot(in FactionSettings settings, Region region, BuildingSlot slot)
    {
        this.PrepareBuildingLevels(settings);

        var result = new List<BuildingLevel>();
        if (slot.SlotType == SlotType.General)
        {
            result.Add(BuildingLevel.Empty);
        }

        var buildings = this.buildings[slot.SlotType][slot.RegionType].Single(x => x.Key == region.ResourceId).Value.AsEnumerable();

        // TODO: Correct a case such as: Quarry + Local Industry + Local Industry
        var used = region.Slots.Where(x => x != slot).Select(x => x.Building).Where(x => x != null).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
        foreach (var entry in used)
        {
            if (buildings.Count(x => x.Value == entry.Key) <= entry.Value)
            {
                buildings = buildings.Where(x => !x.Key.Levels.Contains(entry.Key));
            }
        }

        foreach (var building in buildings.Where(x => !result.Contains(x.Value)))
        {
            result.Add(building.Value);
        }

        return result;
    }

    private void PrepareBuildingLevels(FactionSettings settings)
    {
        var slotTypes = Enums.GetValues<SlotType>();
        var regionTypes = Enums.GetValues<RegionType>();
        var resources = this.buildingBranches.Select(x => x.ResourceId).Distinct();
        this.buildings = slotTypes.ToDictionary(x => x, slotType =>
        {
            return regionTypes.ToDictionary(x => x, regionType =>
            {
                return resources.Select(resourceId =>
                {
                    var levels = this.buildingBranches.SelectMany(x => x.Levels).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
                    var branches = this.buildingBranches.Where(branch =>
                            branch.SlotType == slotType &&
                            (branch.RegionType is null || branch.RegionType == regionType) &&
                            (branch.ReligionId is null || branch.ReligionId == settings.ReligionId) &&
                            (branch.ResourceId is null || branch.ResourceId == resourceId));

                    var unlockedLevels = this.technologyTiers[settings.TechnologyTier].UniversalUnlocks.Except(this.technologyTiers[settings.TechnologyTier].UniversalLocks);
                    if (settings.UseAntilegacyTechnologies)
                    {
                        unlockedLevels = unlockedLevels.Concat(this.technologyTiers[settings.TechnologyTier].AntilegacyUnlocks).Except(this.technologyTiers[settings.TechnologyTier].AntilegacyLocks);
                    }

                    var result = new List<KeyValuePair<BuildingBranch, BuildingLevel>>();
                    foreach (var branch in branches)
                    {
                        foreach (var level in branch.Levels)
                        {
                            if (unlockedLevels.Contains(level))
                            {
                                result.Add(new KeyValuePair<BuildingBranch, BuildingLevel>(branch, level));
                            }
                        }
                    }

                    return new KeyValuePair<int?, List<KeyValuePair<BuildingBranch, BuildingLevel>>>(resourceId, result);
                }).ToList();
            });
        });
    }
}