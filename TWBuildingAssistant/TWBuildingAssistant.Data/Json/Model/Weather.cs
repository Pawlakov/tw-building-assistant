namespace TWBuildingAssistant.Data.Json.Model
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class Weather : IWeather
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
    }
}