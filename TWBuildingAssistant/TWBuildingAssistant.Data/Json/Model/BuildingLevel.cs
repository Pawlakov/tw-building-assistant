namespace TWBuildingAssistant.Data.Json.Model
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class BuildingLevel : IBuildingLevel
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? ParentBuildingLevelId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? RegionalEffectId { get; set; }
    }
}