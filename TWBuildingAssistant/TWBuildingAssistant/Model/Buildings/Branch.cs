namespace TWBuildingAssistant.Model.Buildings
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Newtonsoft.Json;

    public class Branch : IBranch
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
        [DefaultValue(RegionType.Any)]
        public RegionType RegionType { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IList<IBuilding> Levels { get; set; }

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

            foreach (var level in this.Levels)
            {
                if (!level.Validate(out string submessage))
                {
                    message = $"One of the levels is invalid ({submessage}).";
                    return false;
                }
            }

            if (this.Levels.GroupBy(x => x.Id).Any(x => x.Count() > 1))
            {
                message = "Duplicate level.";
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