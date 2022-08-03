namespace TWBuildingAssistant.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EnumsNET;
using TWBuildingAssistant.Data.Model;

public class Faction
{
    private readonly Effect baseFactionwideEffect;

    private readonly TechnologyTier[] technologyTiers;

    private readonly BuildingBranch[] buildingBranches;

    private Dictionary<SlotType, Dictionary<RegionType, List<KeyValuePair<Resource, List<KeyValuePair<BuildingBranch, BuildingLevel>>>>>> buildings;

    private Religion stateReligion;

    private int technologyTier;

    public Faction(string name, IEnumerable<TechnologyTier> technologyTiers, IEnumerable<BuildingBranch> buildingBranches, Effect baseFactionwideEffect = default)
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
        this.technologyTiers = technologyTiers.ToArray();
        this.buildingBranches = buildingBranches.ToArray();
    }

    public string Name { get; }

    public Effect FactionwideEffect =>
        this.baseFactionwideEffect +
        this.stateReligion.EffectWhenState +
        this.technologyTiers[this.technologyTier].UniversalEffect +
        (this.UseAntilegacyTechnologies ? this.technologyTiers[this.technologyTier].AntilegacyEffect : default) +
        new Effect(0, 0, 0, 0, 0, 0, this.FertilityDrop);

    public Religion StateReligion
    {
        get => this.stateReligion;
        set
        {
            if (value == null)
            {
                throw new DomainRuleViolationException("Missing state religion.");
            }

            this.stateReligion = value;
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
                return resources.Select(resource =>
                {
                    var levels = this.buildingBranches.SelectMany(x => x.Levels).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
                    var branches = this.buildingBranches.Where(branch =>
                            branch.SlotType == slotType &&
                            (branch.RegionType == null || branch.RegionType == regionType) &&
                            (branch.Religion == null || branch.Religion == this.StateReligion) &&
                    (branch.Resource == null || branch.Resource == resource));

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

                    return new KeyValuePair<Resource, List<KeyValuePair<BuildingBranch, BuildingLevel>>>(resource, result);
                }).ToList();
            });
        });
    }
}