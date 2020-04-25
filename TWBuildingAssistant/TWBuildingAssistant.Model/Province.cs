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

        public int Food => throw new NotImplementedException();

        public int PublicOrder => throw new NotImplementedException();

        public int ReligiousOsmosis => throw new NotImplementedException();

        public int ResearchRate => throw new NotImplementedException();

        public int Growth => throw new NotImplementedException();

        public double Wealth => throw new NotImplementedException();
    }
}