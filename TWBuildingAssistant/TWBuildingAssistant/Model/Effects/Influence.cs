namespace TWBuildingAssistant.Model.Effects
{
    using System;

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

            if (this.religion != null || !this.ReligionId.HasValue)
            {
                return this.religion;
            }

            this.religion = this.religionParser.Find(this.ReligionId);
            if (this.religion == null)
            {
                throw new EffectsException($"No religion with id = {this.ReligionId.Value}.");
            }

            return this.religion;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReligionId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Value { get; set; }

        public void SetReligionParser(IParser<IReligion> parser)
        {
            this.religionParser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.religion = null;
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