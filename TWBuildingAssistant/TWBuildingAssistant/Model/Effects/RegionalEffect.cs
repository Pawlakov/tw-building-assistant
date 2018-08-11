namespace TWBuildingAssistant.Model.Effects
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a set of effects acting on a region.
    /// </summary>
    public class RegionalEffect : ProvincionalEffect, IRegionalEffect
    {
        /// <summary>
        /// Gets or sets the change of sanitation in only one region.
        /// </summary>
        [Column]
        public int? RegionalSanitation { get; set; }

        /// <summary>
        /// Adds this effect to another one.
        /// </summary>
        /// <param name="other">
        /// The other component of addition.
        /// </param>
        /// <returns>
        /// The <see cref="IRegionalEffect"/> resulting from the addition.
        /// </returns>
        public IRegionalEffect Aggregate(IRegionalEffect other)
        {
            if (other == null)
            {
                other = new RegionalEffect();
            }

            return new RegionalEffect()
                   {
                   RegionalSanitation = (this.RegionalSanitation ?? 0) + (other.RegionalSanitation ?? 0),
                   PublicOrder = (this.PublicOrder ?? 0) + (other.PublicOrder ?? 0),
                   RegularFood = (this.RegularFood ?? 0) + (other.RegularFood ?? 0),
                   FertilityDependentFood = (this.FertilityDependentFood ?? 0) + (other.FertilityDependentFood ?? 0),
                   ProvincionalSanitation = (this.ProvincionalSanitation ?? 0) + (other.ProvincionalSanitation ?? 0),
                   ResearchRate = (this.ResearchRate ?? 0) + (other.ResearchRate ?? 0),
                   Growth = (this.Growth ?? 0) + (other.Growth ?? 0),
                   Fertility = (this.Fertility ?? 0) + (other.Fertility ?? 0),
                   ReligiousOsmosis = (this.ReligiousOsmosis ?? 0) + (other.ReligiousOsmosis ?? 0),
                   Bonuses = this.Bonuses.Concat(other.Bonuses).ToList(),
                   Influences = this.Influences.Concat(other.Influences).ToList()
                   };
        }
    }
}