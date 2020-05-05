namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TWBuildingAssistant.Data.Model;

    public class Faction
    {
        private readonly Effect baseFactionwideEffect;

        private readonly TechnologyTier[] technologyTiers;

        private readonly BuildingBranch[] buildingBranches;

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

            var unlockedLevels = this.technologyTiers[this.technologyTier].UniversalUnlocks.Except(this.technologyTiers[this.technologyTier].UniversalLocks);
            if (this.UseAntilegacyTechnologies)
            {
                unlockedLevels = unlockedLevels.Concat(this.technologyTiers[this.technologyTier].AntilegacyUnlocks).Except(this.technologyTiers[this.technologyTier].AntilegacyLocks);
            }

            // TODO: Correct a case such as: Quarry + Local Industry + Local Industry
            var used = region.Slots.Where(x => x != slot).Select(x => x.Building).Where(x => x != null).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
            var levels = this.buildingBranches.SelectMany(x => x.Levels).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
            var branches = this.buildingBranches.Where(branch =>
                    branch.SlotType == slot.SlotType &&
                    (branch.RegionType == null || branch.RegionType == slot.RegionType) &&
                    (branch.Religion == null || branch.Religion == province.Owner.StateReligion) &&
                    (branch.Resource == null || branch.Resource == region.Resource));
            foreach (var entry in used)
            {
                if (levels.ContainsKey(entry.Key) && levels[entry.Key] <= entry.Value)
                {
                    branches = branches.Where(x => !x.Levels.Contains(entry.Key));
                }
            }

            foreach (var branch in branches)
            {
                foreach (var level in branch.Levels.Where(x => !result.Contains(x)))
                {
                    if (unlockedLevels.Contains(level))
                    {
                        result.Add(level);
                    }
                }
            }

            return result;
        }
    }
}