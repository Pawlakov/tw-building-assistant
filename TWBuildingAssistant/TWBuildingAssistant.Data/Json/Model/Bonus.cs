namespace TWBuildingAssistant.Data.Json.Model
{
    using System.ComponentModel;
    using Newtonsoft.Json;
    using TWBuildingAssistant.Data.Json;
    using TWBuildingAssistant.Data.Model;

    internal class Bonus : IBonus
    {
        [JsonProperty(Required = Required.Always)]
        public int Value { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(JsonEnumConverter))]
        [DefaultValue(IncomeCategory.All)]
        public IncomeCategory Category { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(JsonEnumConverter))]
        [DefaultValue(BonusType.Simple)]
        public BonusType Type { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? RegionalEffectId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? ProvincialEffectId { get; set; }
    }
}