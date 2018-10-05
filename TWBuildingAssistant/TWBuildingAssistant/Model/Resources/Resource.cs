namespace TWBuildingAssistant.Model.Resources
{
    using System.ComponentModel;

    using Newtonsoft.Json;

    public class Resource : IResource
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Obligatory { get; set; }

        [JsonProperty(Required = Required.DisallowNull, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(SlotType.Regular)]
        public SlotType BuildingType { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        public bool Validate(out string message)
        {
            if (this.Name == null)
            {
                message = "Name is null.";
                return false;
            }

            if (this.Name.Equals(string.Empty))
            {
                message = "Name is empty.";
                return false;
            }

            if (this.BuildingType == SlotType.Regular && this.Obligatory)
            {
                message = "Mandatory replacement of general building.";
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