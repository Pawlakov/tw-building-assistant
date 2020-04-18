namespace TWBuildingAssistant.Model
{
    public class Faction
    {
        private Religion stateReligion;

        public Faction(string name, Effect empirewideEffect = default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainRuleViolationException("Faction without name.");
            }

            this.Name = name;
            this.EmpirewideEffect = empirewideEffect;
        }

        public string Name { get; }

        public Effect EmpirewideEffect { get; }

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
    }
}