namespace TWBuildingAssistant.Data.JsonModel
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class BuildingBranchUse : IBuildingBranchUse
    {
        [JsonProperty(Required = Required.Always)]
        public int FactionId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int BuildingBranchId { get; set; }
    }
}