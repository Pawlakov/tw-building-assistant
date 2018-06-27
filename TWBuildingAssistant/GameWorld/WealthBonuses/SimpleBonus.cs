using System;
using System.Globalization;
namespace GameWorld.WealthBonuses
{
	// Najprostszy typ bonusu dochodowego (proste dodanie wartości).
	public class SimpleBonus : WealthBonus
	{
		private int Value { get; }
		//
		public SimpleBonus(int value, WealthCategory category) : base(category)
		{
			if (!ValidateValues(value, category, out string message))
				throw new ArgumentOutOfRangeException(message);
			Value = value;
		}
		// Bonus wykonuje się sam dodając swoją wartość do tablic.
		public override void Execute(int[] values, int[] multipliers, int fertility)
		{
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length != WealthCategoriesCount)
				throw new ArgumentException("Length of values array should be equal to wealth categories count.", "values");
			values[(int)Category] += Value;
		}
		// Sprawdzenie poprawności wartości
		public static bool ValidateValues(int value, WealthCategory category, out string message)
		{
			if (category == WealthCategory.All)
			{
				message = "Simple wealth bonus in 'All' category.";
				return false;
			}
			if (category == WealthCategory.Maintenance && value > 0)
			{
				message = "Positive 'Maintenance' bonus.";
				return false;
			}
			if (category != WealthCategory.Maintenance && value < 0)
			{
				message = "Negative non-maintenance bonus.";
				return false;
			}
			message = "Values are chosen correcly";
			return true;
		}
	}
}
