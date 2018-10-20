namespace TWBuildingAssistant.Model.Climate
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    public class Climate : IClimate
    {
        private IProvincialEffect effect;

        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(JsonConcreteConverter<WeatherEffect[]>))]
        public IEnumerable<IWeatherEffect> WeatherEffects { get; set; }

        public IProvincialEffect GetEffect()
        {
            if (this.effect == null)
            {
                throw new ClimateException("Considered weathers not tracked.");
            }

            return this.effect;
        }

        public void SetWeatherParser(Parser<IWeather> parser)
        {
            foreach (var weatherEffect in this.WeatherEffects)
            {
                weatherEffect.SetWeatherParser(parser);
            }
        }

        public void RefreshEffect()
        {
            var effects = this.WeatherEffects
                .Where(x => x.GetWeather().IsConsidered())
                .Select(x => x.Effect)
                .ToArray();
            this.effect = effects.Any() ? effects.Aggregate((x, y) => x.Aggregate(y)) : new ProvincialEffect();
        }

        public bool Validate(out string message)
        {
            if (this.Name.Equals(string.Empty))
            {
                message = "Name is empty.";
                return false;
            }

            foreach (var weather in this.WeatherEffects.Select(x => x.WeatherId).Distinct())
            {
                if (this.WeatherEffects.Count(x => x.WeatherId == weather) > 1)
                {
                    message = $"Multiple weather effects for weather id = {weather}.";
                    return false;
                }
            }

            foreach (var weatherEffect in this.WeatherEffects)
            {
                if (!weatherEffect.Validate(out string submessage))
                {
                    message = $"Corresponding effect for weather id = {weatherEffect.WeatherId} is invalid ({submessage}).";
                    return false;
                }
            }

            message = "Values are valid.";
            return true;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
