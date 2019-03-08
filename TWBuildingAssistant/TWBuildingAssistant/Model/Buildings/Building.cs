namespace TWBuildingAssistant.Model.Buildings
{
    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Effects;

    public class Building : IBuilding
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(JsonConcreteConverter<RegionalEffect>))]
        public IRegionalEffect Effect { get; set; }

        public bool Validate(out string message)
        {
            if (this.Id == 0)
            {
                message = "Id is 0.";
                return false;
            }

            if (this.Name.Equals(string.Empty))
            {
                message = "Name is empty.";
                return false;
            }

            if (!this.Effect.Validate(out string submessage))
            {
                message = $"Corresponding effect is invalid ({submessage}).";
                return false;
            }

            message = "Values are valid.";
            return true;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}