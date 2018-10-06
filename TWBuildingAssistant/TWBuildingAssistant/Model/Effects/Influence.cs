namespace TWBuildingAssistant.Model.Effects
{
    using System.Linq;

    using Newtonsoft.Json;

    using TWBuildingAssistant.Data;
    using TWBuildingAssistant.Model.Religions;

    public class Influence : IInfluence
    {
        private IReligion religion;

        [JsonIgnore]
        public IReligion Religion
        {
            get
            {
                if (!this.ReligionId.HasValue)
                {
                    return null;
                }

                if (this.religion == null)
                {
                    this.religion = JsonData.Data.Religions.FirstOrDefault(x => x.Id == this.ReligionId);
                }

                return this.religion;
            }
        }

        public int? ReligionId { get; set; }

        public int Value { get; set; }

        public bool Validate(out string message)
        {
            if (this.Value < 1)
            {
                message = "Value is nonpositive.";
                return false;
            }

            message = "Values are correct.";
            return true;
        }

        public override string ToString()
        {
            return $"+{this.Value} {this.Religion?.ToString() ?? "state religion"} religious influence";
        }
    }
}