namespace TWBuildingAssistant.Model.Effects
{
    using System.Linq;

    using Newtonsoft.Json;

    public class RegionalEffect : ProvincialEffect, IRegionalEffect
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RegionalSanitation { get; set; }

        public IRegionalEffect Aggregate(IRegionalEffect other)
        {
            if (other == null)
            {
                other = new RegionalEffect();
            }

            return new RegionalEffect()
                   {
                   RegionalSanitation = this.RegionalSanitation + other.RegionalSanitation,
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
    }
}