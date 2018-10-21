namespace TWBuildingAssistant.Model.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using TWBuildingAssistant.Model.Climate;
    using TWBuildingAssistant.Model.Effects;

    public class Province : IProvince
    {
        private const int MinimalDefaultFertility = 1;

        private const int MaximalDefaultFertility = 6;

        private int currentFertilityDrop;

        private IClimate climate;

        private Parser<IClimate> climateParser;

        private IFertilityDropTracker fertilityDropTracker;

        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Fertility { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ClimateId { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(JsonConcreteConverter<Region[]>))]
        public IEnumerable<IRegion> Regions { get; set; }

        [JsonConverter(typeof(JsonConcreteConverter<ProvincialEffect>))]
        public IProvincialEffect Effect { get; set; } = new ProvincialEffect();

        public int GetCurrentFertility()
        {
            if (this.fertilityDropTracker == null)
            {
                throw new MapException("Fertility drop is not tracked.");
            }

            if (this.Fertility - this.currentFertilityDrop < 0)
            {
                return 0;
            }

            return this.Fertility - this.currentFertilityDrop;
        }

        public IClimate GetClimate()
        {
            if (this.climateParser == null)
            {
                throw new MapException("Climate has not been parsed.");
            }

            if (this.climate != null)
            {
                return this.climate;
            }

            this.climate = this.climateParser.Find(this.ClimateId);
            if (this.climate == null)
            {
                throw new MapException($"No climate with id = {this.ClimateId}.");
            }

            return this.climate;
        }

        public void SetClimateParser(Parser<IClimate> parser)
        {
            this.climateParser = parser ?? throw new ArgumentNullException(nameof(parser));
            this.climate = null;
        }

        public void SetFertilityDropTracker(IFertilityDropTracker tracker)
        {
            this.fertilityDropTracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
            this.fertilityDropTracker.FertilityDropChanged += this.OnFertilityDropChanged;
            this.currentFertilityDrop = this.fertilityDropTracker.FertilityDrop;
        }

        public bool Validate(out string message)
        {
            if (this.Id < 1)
            {
                message = "Invalid id.";
                return false;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                message = "Name is empty.";
                return false;
            }

            if (this.Fertility < MinimalDefaultFertility || this.Fertility > MaximalDefaultFertility)
            {
                message = $"Fertility is out of range ({this.Fertility}).";
                return false;
            }

            if (this.ClimateId < 1)
            {
                message = "Invalid climate id.";
                return false;
            }

            if (!this.Regions.Any())
            {
                message = "There are no regions.";
                return false;
            }

            if (this.Effect == null)
            {
                message = "Corresponding effect is missing.";
                return false;
            }

            if (!this.Effect.Validate(out var submessage))
            {
                message = $"Corresponding effect is invalid ({submessage}).";
                return false;
            }

            if (this.climateParser == null)
            {
                message = "Climate parser is missing.";
                return false;
            }

            message = "Values are correct.";
            return true;
        }

        private void OnFertilityDropChanged(object sender, FertilityDropChangedEventArgs args)
        {
            this.currentFertilityDrop = args.NewFertilityDrop;
        }
    }
}