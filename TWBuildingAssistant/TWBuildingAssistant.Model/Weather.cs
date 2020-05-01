namespace TWBuildingAssistant.Model
{
    public class Weather
    {
        public Weather(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainRuleViolationException("Weather without name.");
            }

            this.Name = name;
        }

        public string Name { get; }
    }
}