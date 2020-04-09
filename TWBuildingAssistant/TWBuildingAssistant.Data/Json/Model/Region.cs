namespace TWBuildingAssistant.Data.Json.Model
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class Region : IRegion
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsCity { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ResourceId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsCoastal { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int SlotsCountOffset { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ProvinceId { get; set; }
    }
}