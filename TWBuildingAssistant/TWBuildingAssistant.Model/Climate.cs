namespace TWBuildingAssistant.Model
{
    using System.Collections.Generic;

    public class Climate
    {
        private readonly IDictionary<Weather, Effect> weatherEffects;

        public Climate(IDictionary<Weather, Effect> weatherEffects)
        {
            this.weatherEffects = new Dictionary<Weather, Effect>();
            var worst = (Effect?)null;
            foreach (var weatherEffect in weatherEffects)
            {
                worst = worst == null ? weatherEffect.Value : Effect.TakeWorse(worst.Value, weatherEffect.Value);
                this.weatherEffects.Add(weatherEffect.Key, worst.Value);
            }
        }

        public Effect GetEffectForWorstCaseWeather(Weather worstCaseWeather)
        {
            return this.weatherEffects[worstCaseWeather];
        }
    }
}