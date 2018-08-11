namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a set of effects acting on an entire province.
    /// </summary>
    public interface IProvincionalEffect
    {
        /// <summary>
        /// Gets the change in province's public order.
        /// </summary>
        int? PublicOrder { get; }

        /// <summary>
        /// Gets the change in province's food regardless of the fertility.
        /// </summary>
        int? RegularFood { get; }

        /// <summary>
        /// Gets the change in province's food dependent on the fertility level.
        /// </summary>
        int? FertilityDependentFood { get; }

        /// <summary>
        /// Gets the change in entire province's sanitation.
        /// </summary>
        int? ProvincionalSanitation { get; }

        /// <summary>
        /// Gets the change in research rate in percents.
        /// </summary>
        int? ResearchRate { get; }

        /// <summary>
        /// Gets the change in province's growth.
        /// </summary>
        int? Growth { get; }

        /// <summary>
        /// Gets the change in province's fertility.
        /// </summary>
        int? Fertility { get; }

        /// <summary>
        /// Gets the change in province's religious osmosis.
        /// </summary>
        int? ReligiousOsmosis { get; }

        /// <summary>
        /// Gets the set of wealth bonuses belonging to this effect.
        /// </summary>
        IEnumerable<IBonus> Bonuses { get; }

        /// <summary>
        /// Gets the set of religious influences belonging to this effect.
        /// </summary>
        IEnumerable<IInfluence> Influences { get; }

        /// <summary>
        /// Calculates the actual value of effect on food at the given fertility level.
        /// </summary>
        /// <param name="fertility">
        /// The fertility level.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> value representing the total effect on food.
        /// </returns>
        int Food(int fertility);

        /// <summary>
        /// Adds this effect to another one.
        /// </summary>
        /// <param name="other">
        /// The other component of addition.
        /// </param>
        /// <returns>
        /// The <see cref="IProvincionalEffect"/> resulting from the addition.
        /// </returns>
        IProvincionalEffect Aggregate(IProvincionalEffect other);

        /// <summary>
        /// Returns a value indicating whether the current combination of values is valid.
        /// </summary>
        /// <param name="message">
        /// Optional message containing details of vaildation's result.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> indicating result of validation.
        /// </returns>
        bool Validate(out string message);
    }
}