namespace TWBuildingAssistant.Model.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    public class ProvincialEffect : IProvincialEffect
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int PublicOrder { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RegularFood { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FertilityDependentFood { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ProvincialSanitation { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ResearchRate { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Growth { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Fertility { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ReligiousOsmosis { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(JsonConcreteConverter<Bonus[]>))]
        public IEnumerable<IBonus> Bonuses { get; set; } = new IBonus[0];

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(JsonConcreteConverter<Influence[]>))]
        public IEnumerable<IInfluence> Influences { get; set; } = new IInfluence[0];

        public int Food(int fertilityLevel)
        {
            return this.RegularFood + (this.FertilityDependentFood * fertilityLevel);
        }

        public IProvincialEffect Aggregate(IProvincialEffect other)
        {
            if (other == null)
            {
                other = new ProvincialEffect();
            }

            return new ProvincialEffect()
                   {
                   PublicOrder = this.PublicOrder + other.PublicOrder,
                   RegularFood = this.RegularFood + other.RegularFood,
                   FertilityDependentFood = this.FertilityDependentFood + other.FertilityDependentFood,
                   ProvincialSanitation = this.ProvincialSanitation + other.ProvincialSanitation,
                   ResearchRate = this.ResearchRate + other.ResearchRate,
                   Growth = this.Growth + other.Growth,
                   Fertility = this.Fertility + other.Fertility,
                   ReligiousOsmosis = this.ReligiousOsmosis + other.ReligiousOsmosis,
                   Bonuses = this.Bonuses.Concat(other.Bonuses).ToList(),
                   Influences = this.Influences.Concat(other.Influences).ToList()
                   };
        }

        public bool Validate(out string message)
        {
            if (this.Influences == null)
            {
                message = $"Influences are missing.";
                return false;
            }

            if (this.Bonuses == null)
            {
                message = $"Bonuses are missing.";
                return false;
            }

            var subMessage = string.Empty;
            if (!this.Influences.All(influence => influence.Validate(out subMessage)))
            {
                message = $"On of influences is invalid ({subMessage}).";
                return false;
            }

            if (!this.Bonuses.All(bonus => bonus.Validate(out subMessage)))
            {
                message = $"On of bonuses is invalid ({subMessage}).";
                return false;
            }

            message = "Values are valid";
            return true;
        }

        public IProvincialEffect TakeWorst(IProvincialEffect other)
        {
            if (other == null)
            {
                other = new ProvincialEffect();
            }

            return new ProvincialEffect()
                       {
                           PublicOrder = Math.Min(this.PublicOrder, other.PublicOrder),
                           RegularFood = Math.Min(this.RegularFood, other.RegularFood),
                           FertilityDependentFood = Math.Min(this.FertilityDependentFood, other.FertilityDependentFood),
                           ProvincialSanitation = Math.Min(this.ProvincialSanitation, other.ProvincialSanitation),
                           ResearchRate = Math.Min(this.ResearchRate, other.ResearchRate),
                           Growth = Math.Min(this.Growth, other.Growth),
                           Fertility = Math.Min(this.Fertility, other.Fertility),
                           ReligiousOsmosis = Math.Min(this.ReligiousOsmosis, other.ReligiousOsmosis),
                           Bonuses = this.Bonuses.TakeWorst(other.Bonuses).ToList(),
                           Influences = this.Influences.TakeWorst(other.Influences).ToList()
                       };
        }
    }
}