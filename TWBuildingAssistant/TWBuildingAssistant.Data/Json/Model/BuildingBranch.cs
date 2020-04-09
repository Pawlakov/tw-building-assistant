namespace TWBuildingAssistant.Data.Json.Model
{
    using System.ComponentModel;
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    internal class BuildingBranch : IBuildingBranch
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(JsonEnumConverter))]
        [DefaultValue(SlotType.General)]
        public SlotType SlotType { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(JsonEnumConverter))]
        [DefaultValue(null)]
        public RegionType? RegionType { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool AllowParallel { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int RootBuildingLevelId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? ReligionId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? ResourceId { get; set; }
    }
}