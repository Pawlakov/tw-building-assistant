namespace TWBuildingAssistant.Domain.Models;

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
    private readonly Influence baseFactionwideInfluence;

    private readonly TechnologyTier[] technologyTiers;

    private readonly BuildingBranch[] buildingBranches;

    private Dictionary<SlotType, Dictionary<RegionType, Dictionary<Resource, List<KeyValuePair<BuildingBranch, BuildingLevel>>>>> buildings;

    private int? stateReligionId;

    private int technologyTier;

    public Faction(string name, IEnumerable<TechnologyTier> technologyTiers, IEnumerable<BuildingBranch> buildingBranches, Effect baseFactionwideEffect, IEnumerable<Income> baseFactionwideIncomes, Influence baseFactionwideInfluence)
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
        this.baseFactionwideInfluence = baseFactionwideInfluence;
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

    public Influence GetFactionwideInfluence(ImmutableArray<Religion> religions)
    {
        return
            this.baseFactionwideInfluence +
            new Influence(null, religions.Single(x => x.Id == this.stateReligionId).StateInfluenceWhenState) +
            this.technologyTiers[this.technologyTier].UniversalInfluence +
            (this.UseAntilegacyTechnologies ? this.technologyTiers[this.technologyTier].AntilegacyInfluence : default);
    }

    public int? StateReligionId
    {
        get => this.stateReligionId;
        set
        {
            if (value == null)
            {
                throw new DomainRuleViolationException("Missing state religion.");
            }

            this.stateReligionId = value;
            this.PrepareBuildingLevels();
        }
    }

    public int TechnologyTier
    {
        get => this.technologyTier;
        set
        {
            if (value < 0 || value > 4)
            {
                throw new DomainRuleViolationException("Tech tier out of range.");
            }

            this.technologyTier = value;
            this.PrepareBuildingLevels();
        }
    }

    public bool UseAntilegacyTechnologies { get; set; }

    public int FertilityDrop { get; set; }

    public IEnumerable<BuildingLevel> GetBuildingLevelsForSlot(Province province, Region region, BuildingSlot slot)
    {
        var result = new List<BuildingLevel>();
        if (slot.SlotType == SlotType.General)
        {
            result.Add(BuildingLevel.Empty);
        }

        var buildings = this.buildings[slot.SlotType][slot.RegionType].Single(x => x.Key == region.Resource).Value.AsEnumerable();

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
        var resources = this.buildingBranches.Select(x => x.Resource).Distinct();
        this.buildings = slotTypes.ToDictionary(x => x, slotType =>
        {
            return regionTypes.ToDictionary(x => x, regionType =>
            {
                return resources.ToDictionary(x => x, resource =>
                {
                    var levels = this.buildingBranches.SelectMany(x => x.Levels).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
                    var branches = this.buildingBranches.Where(branch =>
                            branch.SlotType == slotType &&
                            (branch.RegionType == null || branch.RegionType == regionType) &&
                            (branch.ReligionId == null || branch.ReligionId == this.StateReligionId) &&
                            (branch.Resource == default || branch.Resource == resource));

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

                    return result;
                });
            });
        });
    }
}