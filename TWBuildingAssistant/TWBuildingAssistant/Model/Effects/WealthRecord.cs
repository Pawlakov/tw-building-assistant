namespace TWBuildingAssistant.Model.Effects
{
    /// <summary>
    /// Represents cumulated bonuses from a single wealth category. Allows to calculate the total wealth from this category.
    /// </summary>
    public class WealthRecord
    {
        /// <summary>
        /// The cumulated values of wealth bonuses, of all types, within one wealth category.
        /// </summary>
        private readonly int[] values = new int[3];

        /// <summary>
        /// Gets or sets the total value of bonuses of a given type.
        /// </summary>
        /// <param name="index">
        /// The <see cref="BonusType"/> for which the value is accessed.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> being total value of bonuses of a given type.
        /// </returns>
        public int this[BonusType index]
        {
            get => this.values[(int)index];
            set => this.values[(int)index] = value;
        }

        /// <summary>
        /// Calculates the wealth from one category at the given fertility level.
        /// </summary>
        /// <param name="fertility">
        /// The fertility level.
        /// </param>
        /// <returns>
        /// The <see cref="double"/> being the resulting value.
        /// </returns>
        public double Calculate(int fertility)
        {
            return (this[BonusType.Simple] + (this[BonusType.FertilityDependent] * fertility)) * ((100 + this[BonusType.Percentage]) * 0.01);
        }
    }
}