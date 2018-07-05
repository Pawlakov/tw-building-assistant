using System.Collections.Generic;
using System.Linq;
using EnumsNET;

namespace GameWorld.Effects
{
	public class Wealth
	{
		// Instance members:

		private IEnumerable<WealthBonus> _bonuses;

		public Wealth()
		{
			_bonuses = new List<WealthBonus>();
		}

		public double TotalWealth(int fertilityLevel)
		{
			var result = 0.0;
			var records = new Dictionary<WealthCategory, WealthRecord>(CategoriesCount);
			foreach (var bonus in _bonuses)
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

		public void AddBonus(WealthBonus bonus)
		{
			_bonuses = _bonuses.Append(bonus);
		}

		public void AddBonuses(IEnumerable<WealthBonus> bonuses)
		{
			_bonuses = _bonuses.Concat(bonuses);
		}

		// Classifier members:

		public static int CategoriesCount { get; } = Enums.GetMemberCount<WealthCategory>();

		public static IEnumerable<WealthCategory> Categories { get; } = Enums.GetValues<WealthCategory>();
	}
}