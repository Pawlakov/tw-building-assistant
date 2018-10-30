namespace TWBuildingAssistant.Model.Weather
{
    using System.Linq;

    using Newtonsoft.Json;

    public class Weather : IWeather
    {
        private IConsideredWeatherTracker consideredWeatherTracker;

        private bool isConsidered;
        
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsConsideredByDefault { get; set; }

        [JsonIgnore]
        public IConsideredWeatherTracker ConsideredWeatherTracker
        {
            set
            {
                this.consideredWeatherTracker = value;
                this.consideredWeatherTracker.ConsideredWeatherChanged += this.OnConsideredWeatherChanged;
                this.isConsidered = value.ConsideredWeathers.Contains(this);
            }
        }

        public bool IsConsidered()
        {
            if (this.consideredWeatherTracker == null)
            {
                throw new WeatherException("Considered weather not tracked.");
            }

            return this.isConsidered;
        }

        public bool Validate(out string message)
        {
            if (this.Id < 1)
            {
                message = "Id is out of range.";
                return false;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                message = "Name is empty or null.";
                return false;
            }

            message = "Values are valid.";
            return true;
        }

        public override string ToString()
        {
            return this.Name;
        }

        private void OnConsideredWeatherChanged(object sender, ConsideredWeatherChangedArgs e)
        {
            this.isConsidered = e.NewConsideredWeathers.Contains(this);
        }
    }
}