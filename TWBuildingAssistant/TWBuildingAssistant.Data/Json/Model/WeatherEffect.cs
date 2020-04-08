namespace TWBuildingAssistant.Data.JsonModel
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class WeatherEffect : IWeatherEffect
    {
        [JsonProperty(Required = Required.Always)]
        public int ClimateId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int WeatherId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ProvincialEffectId { get; set; }
    }
}