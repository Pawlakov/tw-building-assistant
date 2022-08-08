﻿namespace TWBuildingAssistant.Domain.OldModels;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using EnumsNET;
using TWBuildingAssistant.Data.Model;
using TWBuildingAssistant.Domain;
using TWBuildingAssistant.Domain.Exceptions;

public class Faction
{
    private readonly Effect baseFactionwideEffect;
    private readonly Income[] baseFactionwideIncomes;
    private readonly Influence[] baseFactionwideInfluences;

    private readonly TechnologyTier[] technologyTiers;

    private readonly BuildingBranch[] buildingBranches;

    private Dictionary<SlotType, Dictionary<RegionType, List<KeyValuePair<int?, List<KeyValuePair<BuildingBranch, BuildingLevel>>>>>> buildings;

    public Faction(string name, IEnumerable<TechnologyTier> technologyTiers, IEnumerable<BuildingBranch> buildingBranches, Effect baseFactionwideEffect, IEnumerable<Income> baseFactionwideIncomes, IEnumerable<Influence> baseFactionwideInfluences)
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

        this.Name = name;
        this.baseFactionwideEffect = baseFactionwideEffect;
        this.baseFactionwideIncomes = baseFactionwideIncomes.ToArray();
        this.baseFactionwideInfluences = baseFactionwideInfluences.ToArray();
        this.technologyTiers = technologyTiers.ToArray();
        this.buildingBranches = buildingBranches.ToArray();
    }

    public string Name { get; }

    public IEnumerable<Effect> GetFactionwideEffects(ImmutableArray<Religion> religions)
    {
        return
            this.technologyTiers[this.technologyTier].UniversalEffects
            .Concat(this.UseAntilegacyTechnologies ? this.technologyTiers[this.technologyTier].AntilegacyEffects : Enumerable.Empty<Effect>())
            .Append(this.baseFactionwideEffect)
            .Append(religions.Single(x => x.Id == this.stateReligionId).EffectWhenState)
            .Append(EffectOperations.Create(fertility: this.FertilityDrop));
    }

    public IEnumerable<Income> GetFactionwideIncomes(ImmutableArray<Religion> religions)
    {
        return
            new[]
            {
                this.baseFactionwideIncomes,
                religions.Single(x => x.Id == this.stateReligionId).IncomesWhenState,
                this.technologyTiers[this.technologyTier].UniversalIncomes,
                (this.UseAntilegacyTechnologies ? this.technologyTiers[this.technologyTier].AntilegacyIncomes : Enumerable.Empty<Income>()),
            }.SelectMany(x => x);
    }

    public IEnumerable<Influence> GetFactionwideInfluence(ImmutableArray<Religion> religions)
    {
        return
            new[]
            {
                this.baseFactionwideInfluences,
                this.technologyTiers[this.technologyTier].UniversalInfluences,
                (this.UseAntilegacyTechnologies ? this.technologyTiers[this.technologyTier].AntilegacyInfluences : Enumerable.Empty<Influence>()),
            }.SelectMany(x => x)
            .Append(InfluenceOperations.Create(null, religions.Single(x => x.Id == this.stateReligionId).StateInfluenceWhenState));
    }

    public IEnumerable<BuildingLevel> GetBuildingLevelsForSlot(Region region, BuildingSlot slot)
    {
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

    private void PrepareBuildingLevels()
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
                            (branch.ReligionId is null || branch.ReligionId == this.StateReligionId) &&
                            (branch.ResourceId is null || branch.ResourceId == resourceId));

                    var unlockedLevels = this.technologyTiers[this.technologyTier].UniversalUnlocks.Except(this.technologyTiers[this.technologyTier].UniversalLocks);
                    if (this.UseAntilegacyTechnologies)
                    {
                        unlockedLevels = unlockedLevels.Concat(this.technologyTiers[this.technologyTier].AntilegacyUnlocks).Except(this.technologyTiers[this.technologyTier].AntilegacyLocks);
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