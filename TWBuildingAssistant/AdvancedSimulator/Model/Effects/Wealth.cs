namespace TWBuildingAssistant.Model.Effects
{
    using System.Collections.Generic;
    using System.Linq;

    public class Wealth
    {
        private IEnumerable<IBonus> bonuses;

        public Wealth()
        {
            this.bonuses = new List<IBonus>();
        }

        public double TotalWealth(int fertilityLevel)
        {
            var result = 0.0;
            var records = new Dictionary<WealthCategory, WealthRecord>();
            foreach (var bonus in this.bonuses)
                bonus.Execute(records);
            if (records.ContainsKey(WealthCategory.All))
            {
                foreach (var record in records)
                    if (record.Key != WealthCategory.All && record.Key != WealthCategory.Maintenance)
                        record.Value.AddToPercents(records[WealthCategory.All].Percents);
            }

            foreach (var record in records)
                result += record.Value.Calculate(fertilityLevel);
            return result;
        }

        public void AddBonus(IBonus bonus)
        {
            this.bonuses = this.bonuses.Append(bonus);
        }

        public void AddBonuses(IEnumerable<IBonus> bonuses)
        {
            this.bonuses = this.bonuses.Concat(bonuses);
        }
    }
}