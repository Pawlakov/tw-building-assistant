namespace TWBuildingAssistant.Data.JsonModel
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class Province : IProvince
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Fertility { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ClimateId { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}