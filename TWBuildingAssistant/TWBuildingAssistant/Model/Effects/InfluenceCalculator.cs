namespace TWBuildingAssistant.Model.Effects
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the calculator of a change in public order caused by religions within a single province.
    /// </summary>
    public static class InfluenceCalculator
    {
        /// <summary>
        /// Calculates the change in public order caused by the given set of <see cref="IInfluence"/>s.
        /// </summary>
        /// <param name="influences">
        /// All religious <see cref="IInfluence"/>s within one province.
        /// </param>
        /// <returns>
        /// The public order change resulting from religious influences.
        /// </returns>
        public static int PublicOrder(IEnumerable<IInfluence> influences)
        {
            var state = 0;
            var other = 0;
            foreach (var influence in influences)
            {
                if (influence.Religion == null || influence.Religion.IsState)
                {
                    state += influence.Value;
                }
                else
                {
                    other += influence.Value;
                }
            }

            var percentage = 100 * (state / (other + (double)state));

            // return -(int)Math.Floor((percentage * -0.06993007) + 7.5);
            var intermediateValue = (750 - (percentage * 7)) * 0.01;
            return -(int)Math.Floor(intermediateValue);
        }
    }
}
