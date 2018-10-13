namespace TWBuildingAssistant.Model.Climate
{
    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    public class WeatherEffect : IWeatherEffect
    {
        private IWeather weather;

        private IParser<IWeather> weatherParser;

        [JsonProperty(Required = Required.Always)]
        public int WeatherId { get; set; }

        [JsonIgnore]
        public IWeather Weather
        {
            get
            {
                if (this.weatherParser == null)
                {
                    throw new ClimateException("Weather has not been parsed.");
                }

                return this.weather;
            }
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(JsonConcreteConverter<ProvincialEffect>))]
        public IProvincialEffect Effect { get; set; } = new ProvincialEffect();

        [JsonIgnore]
        public IParser<IWeather> WeatherParser
        {
            set
            {
                this.weatherParser = value;
                this.weather = this.weatherParser.Find(this.WeatherId);
            }
        }

        public bool Validate(out string message)
        {
            if (this.Effect.Validate(out string submessage))
            {
                message = $"Corresponding effect for weather is in invalid ({submessage}).";
                return false;
            }

            message = "Values are valid.";
            return true;
        }

        public override string ToString()
        {
            return $"Weather: {this.Weather?.Name ?? this.WeatherId.ToString()} Effect: {this.Effect}";
        }
    }
}