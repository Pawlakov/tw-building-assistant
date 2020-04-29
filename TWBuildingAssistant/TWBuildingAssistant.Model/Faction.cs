namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;
    using System.Linq;

    public class Faction
    {
        private readonly Effect baseFactionwideEffect;

        private readonly TechnologyTier[] technologyTiers;

        private Religion stateReligion;

        private int technologyTier;

        public Faction(string name, IEnumerable<TechnologyTier> technologyTiers, Effect baseFactionwideEffect = default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainRuleViolationException("Faction without name.");
            }

            if (technologyTiers == null)
            {
                throw new DomainRuleViolationException("Missing tech tiers.");
            }

            if (technologyTiers.Count() != 4)
            {
                throw new DomainRuleViolationException("Invalid tech tiers count.");
            }

            this.Name = name;
            this.baseFactionwideEffect = baseFactionwideEffect;
            this.technologyTiers = technologyTiers.ToArray();
        }

        public string Name { get; }

        public Effect FactionwideEffect =>
            this.baseFactionwideEffect +
            this.stateReligion.EffectWhenState +
            this.technologyTiers[this.technologyTier - 1].UniversalEffect +
            (this.UseAntilegacyTechnologies ? this.technologyTiers[this.technologyTier - 1].AntilegacyEffect : default) +
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
                if (value < 1 || value > 4)
                {
                    throw new DomainRuleViolationException("Tech tier out of range.");
                }

                this.technologyTier = value;
            }
        }

        public bool UseAntilegacyTechnologies { get; set; }

        public int FertilityDrop { get; set; }
    }
}