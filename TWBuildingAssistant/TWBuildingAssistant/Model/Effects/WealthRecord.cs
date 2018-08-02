namespace TWBuildingAssistant.Model.Effects
{
    /// <summary>
    /// Represents summed up bonuses from a single wealth category. Allows to calculate the total wealth from this category.
    /// </summary>
    public class WealthRecord
    {
        /// <summary>
        /// Gets or sets the base value of income.
        /// </summary>
        public int BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the ammount percents added to the base value and fertility depended value combined.
        /// </summary>
        public int Percents { get; set; }

        /// <summary>
        /// Gets or sets the value added for every fertility level.
        /// </summary>
        public int ValuePerFertilityLevel { get; set; }

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
            return (this.BaseValue + (this.ValuePerFertilityLevel * fertility)) * ((100 + this.Percents) * 0.01);
        }
    }
}