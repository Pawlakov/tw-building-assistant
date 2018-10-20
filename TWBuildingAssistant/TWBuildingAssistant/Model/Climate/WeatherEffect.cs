namespace TWBuildingAssistant.Model.Climate
{
    using System;

    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    public class WeatherEffect : IWeatherEffect
    {
        private IWeather weather;

        private Parser<IWeather> weatherParser;

        [JsonProperty(Required = Required.Always)]
        public int WeatherId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(JsonConcreteConverter<ProvincialEffect>))]
        public IProvincialEffect Effect { get; set; } = new ProvincialEffect();

        public IWeather GetWeather()
        {
            if (this.weatherParser == null)
            {
                throw new ClimateException("Weather has not been parsed.");
            }

            if (this.weather != null)
            {
                return this.weather;
            }

            this.weather = this.weatherParser.Find(this.WeatherId);
            if (this.weather == null)
            {
                throw new ClimateException($"No weather with id = {this.WeatherId}.");
            }

            return this.weather;
        }

        public void SetWeatherParser(Parser<IWeather> parser)
        {
            this.weatherParser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.weather = null;
        }

        public bool Validate(out string message)
        {
            if (this.Effect == null)
            {
                message = "Corresponding effect for weather is missing.";
                return false;
            }

            if (!this.Effect.Validate(out var submessage))
            {
                message = $"Corresponding effect for weather is invalid ({submessage}).";
                return false;
            }

            if (this.weatherParser == null)
            {
                message = "Weather parser is missing.";
                return false;
            }

            message = "Values are valid.";
            return true;
        }

        public override string ToString()
        {
            return $"Weather: {this.GetWeather()?.Name ?? this.WeatherId.ToString()} Effect: {this.Effect}";
        }
    }
}