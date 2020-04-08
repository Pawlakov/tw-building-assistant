namespace TWBuildingAssistant.Data.JsonModel
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class BuildingLevelLock : IBuildingLevelLock
    {
        [JsonProperty(Required = Required.Always)]
        public int BuildingLevelId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int TechnologyLevelId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Antilegacy { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Lock { get; set; }
    }
}