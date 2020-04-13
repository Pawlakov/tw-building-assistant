namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;
    using System.Linq;

    public static class WealthCalculator
    {
        public static double CalculateTotalWealth(IEnumerable<IBonus> bonuses, int fertilityLevel)
        {
            var records = new Dictionary<WealthCategory, WealthRecord>();
            foreach (var bonus in bonuses)
            {
                if (!records.ContainsKey(bonus.Category))
                {
                    records.Add(bonus.Category, new WealthRecord());
                }

                records[bonus.Category][bonus.Type] += bonus.Value;
            }

            if (records.ContainsKey(WealthCategory.All))
            {
                foreach (var record in records.Where(x => x.Key != WealthCategory.All && x.Key != WealthCategory.Maintenance).Select(x => x.Value))
                {
                    record[BonusType.Percentage] += records[WealthCategory.All][BonusType.Percentage];
                }
            }

            var result = records.Select(x => x.Value.Calculate(fertilityLevel)).Sum();
            return result;
        }
    }
}