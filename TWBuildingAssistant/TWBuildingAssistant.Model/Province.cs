namespace TWBuildingAssistant.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Province
    {
        private readonly Effect baseEffect;

        public Province(string name, IEnumerable<Region> regions, Effect baseEffect = default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainRuleViolationException("Province without name.");
            }

            if (regions == null)
            {
                throw new DomainRuleViolationException("No regions given for a province.");
            }

            if (regions.Count() != 3)
            {
                throw new DomainRuleViolationException("Invalid region count.");
            }

            this.Name = name;
            this.baseEffect = baseEffect;
            this.Regions = regions.ToList();
        }

        public string Name { get; }

        public IEnumerable<Region> Regions { get; }

        public Faction Owner { get; set; }

        public ProvinceState State
        {
            get
            {
                var regionalEffects = this.Regions.Select(x => x.Effect);
                var effect = regionalEffects.Aggregate(this.baseEffect + this.Owner.FactionwideEffect, (x, y) => x + y);

                var sanitation = regionalEffects.Select(x => x.RegionalSanitation + effect.ProvincialSanitation);
                var food = effect.RegularFood + (effect.Fertility * effect.FertilityDependentFood);
                var publicOrder = effect.PublicOrder + effect.Influence.PublicOrder(this.Owner.StateReligion);
                var income = effect.Income.GetIncome(effect.Fertility);

                return new ProvinceState(sanitation, food, publicOrder, effect.ReligiousOsmosis, effect.ResearchRate, effect.Growth, income);
            }
        }
    }
}