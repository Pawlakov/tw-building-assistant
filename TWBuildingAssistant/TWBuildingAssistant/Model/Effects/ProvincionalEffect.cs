namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    /// <summary>
    /// Represents a set of effects acting on an entire province.
    /// </summary>
    public class ProvincionalEffect : IProvincionalEffect
    {
        /// <summary>
        /// Gets or sets the change in province's public order.
        /// </summary>
        [Column]
        public int? PublicOrder { get; set; }

        /// <summary>
        /// Gets or sets the change in province's food regardless of the fertility.
        /// </summary>
        [Column]
        public int? RegularFood { get; set; }

        /// <summary>
        /// Gets or sets the change in province's food dependent on the fertility level.
        /// </summary>
        [Column]
        public int? FertilityDependentFood { get; set; }

        /// <summary>
        /// Gets or sets the change in entire province's sanitation.
        /// </summary>
        [Column]
        public int? ProvincionalSanitation { get; set; }

        /// <summary>
        /// Gets or sets the change in research rate in percents.
        /// </summary>
        [Column]
        public int? ResearchRate { get; set; }

        /// <summary>
        /// Gets or sets the change in province's growth.
        /// </summary>
        [Column]
        public int? Growth { get; set; }

        /// <summary>
        /// Gets or sets the change in province's fertility.
        /// </summary>
        [Column]
        public int? Fertility { get; set; }

        /// <summary>
        /// Gets or sets the change in province's religious osmosis.
        /// </summary>
        [Column]
        public int? ReligiousOsmosis { get; set; }

        /// <summary>
        /// Gets or sets the set of wealth bonuses belonging to this effect.
        /// </summary>
        [NotMapped]
        public IEnumerable<IBonus> Bonuses { get; set; } = new List<IBonus>();

        /// <summary>
        /// Gets or sets the set of religious influences belonging to this effect.
        /// </summary>
        [NotMapped]
        public IEnumerable<IInfluence> Influences { get; set; } = new List<IInfluence>();

        /// <summary>
        /// Calculates the actual value of effect on food at the given fertility level.
        /// </summary>
        /// <param name="fertility">
        /// The fertility level.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> value representing the total effect on food.
        /// </returns>
        public int Food(int fertility)
        {
            return (this.RegularFood ?? 0) + ((this.FertilityDependentFood ?? 0) * fertility);
        }

        /// <summary>
        /// Adds this effect to another one.
        /// </summary>
        /// <param name="other">
        /// The other component of addition.
        /// </param>
        /// <returns>
        /// The <see cref="IProvincionalEffect"/> resulting from the addition.
        /// </returns>
        public IProvincionalEffect Aggregate(IProvincionalEffect other)
        {
            if (other == null)
            {
                other = new ProvincionalEffect();
            }

            return new ProvincionalEffect()
                   {
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

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> indicating result of validation.
        /// </returns>
        public bool Validate(out string message)
        {
            var submessage = string.Empty;
            if (!this.Influences.All(influence => influence.Validate(out submessage)))
            {
                message = $"On of influences is invalid ({submessage}).";
                return false;
            }

            if (!this.Bonuses.All(bonus => bonus.Validate(out submessage)))
            {
                message = $"On of bonuses is invalid ({submessage}).";
                return false;
            }

            message = "Values are valid";
            return true;
        }
    }
}