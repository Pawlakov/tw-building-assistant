namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the calculator of total wealth of single province.
    /// </summary>
    public static class WealthCalculator
    {
        /// <summary>
        /// Calculates the total wealth of a single province.
        /// </summary>
        /// <param name="bonuses">
        /// The set of all bonuses influencing this province.
        /// </param>
        /// <param name="fertilityLevel">
        /// The province's fertility level.
        /// </param>
        /// <returns>
        /// The <see cref="double"/> value being the result of calculation.
        /// </returns>
        public static double CalculateTotalWealth(IEnumerable<IBonus> bonuses, int fertilityLevel)
        {
            var result = 0.0;
            var records = new Dictionary<WealthCategory, WealthRecord>();
            foreach (var bonus in bonuses)
            {
                bonus.Execute(records);
            }

            if (records.ContainsKey(WealthCategory.All))
            {
                foreach (var record in
                from KeyValuePair<WealthCategory, WealthRecord> pair in records
                where pair.Key != WealthCategory.All && pair.Key != WealthCategory.Maintenance
                select pair.Value)
                {
                    record.Percents += records[WealthCategory.All].Percents;
                }
            }

            foreach (var record in records)
            {
                result += record.Value.Calculate(fertilityLevel);
            }

            return result;
        }
    }
}