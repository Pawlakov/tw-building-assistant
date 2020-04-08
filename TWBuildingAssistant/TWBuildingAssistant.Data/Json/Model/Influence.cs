namespace TWBuildingAssistant.Data.JsonModel
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class Influence : IInfluence
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? ReligionId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Value { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? RegionalEffectId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? ProvincialEffectId { get; set; }
    }
}