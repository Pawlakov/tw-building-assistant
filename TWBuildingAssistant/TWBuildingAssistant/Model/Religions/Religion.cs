namespace TWBuildingAssistant.Model.Religions
{
    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Effects;

    public class Religion : IReligion
    {
        private IStateReligionTracker stateReligionTracker;

        private bool isState;

        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(JsonConcreteConverter<ProvincialEffect>))]
        public IProvincialEffect Effect { get; set; } = new ProvincialEffect();

        [JsonIgnore]
        public bool IsState
        {
            get
            {
                if (this.stateReligionTracker == null)
                {
                    throw new ReligionsException("State religion not tracked.");
                }

                return this.isState;
            }
        }

        [JsonIgnore]
        public IStateReligionTracker StateReligionTracker
        {
            set
            {
                this.stateReligionTracker = value;
                this.stateReligionTracker.StateReligionChanged += this.OnStateReligionChanged;
            }
        }

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

            if (!this.Effect.Validate(out string submessage))
            {
                message = $"Corresponding effect is in invalid ({submessage}).";
                return false;
            }

            message = "Values are valid.";
            return true;
        }

        public override string ToString()
        {
            return this.Name;
        }

        private void OnStateReligionChanged(object sender, StateReligionChangedArgs e)
        {
            this.isState = ReferenceEquals(this, e.NewStateReligion);
        }
    }
}