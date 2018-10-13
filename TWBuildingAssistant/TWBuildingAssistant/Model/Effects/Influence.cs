namespace TWBuildingAssistant.Model.Effects
{
    using Newtonsoft.Json;
    
    using TWBuildingAssistant.Model.Religions;

    public class Influence : IInfluence
    {
        private IReligion religion;

        private IParser<IReligion> religionParser;

        public IReligion GetReligion()
        {
            if (this.religionParser == null)
            {
                throw new EffectsException("Religion has not been parsed.");
            }

            return this.religion;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReligionId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Value { get; set; }

        [JsonIgnore]
        public IParser<IReligion> ReligionParser
        {
            set
            {
                this.religionParser = value;
                this.religion = this.religionParser.Find(ReligionId);
            }
        }

        public bool Validate(out string message)
        {
            if (this.ReligionId.HasValue && this.ReligionId.Value < 1)
            {
                message = "Religion id is out od range.";
                return false;
            }

            if (this.religionParser == null)
            {
                message = "Religion parser is missing.";
                return false;
            }

            if (this.Value < 1)
            {
                message = "Value is out od range.";
                return false;
            }

            message = "Values are correct.";
            return true;
        }

        public override string ToString()
        {
            return $"+{this.Value} {this.GetReligion()?.ToString() ?? "state religion"} religious influence";
        }
    }
}