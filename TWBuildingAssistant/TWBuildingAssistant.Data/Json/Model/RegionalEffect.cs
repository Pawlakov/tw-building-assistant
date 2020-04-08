namespace TWBuildingAssistant.Data.JsonModel
{
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Model;

    internal class RegionalEffect : ProvincialEffect, IRegionalEffect
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RegionalSanitation { get; set; }
    }
}