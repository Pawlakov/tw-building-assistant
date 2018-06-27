using System;
using System.Globalization;
namespace GameWorld.WealthBonuses
{
	public class MultiplierBonus : WealthBonus
	{
		// Wartość wyrażona w procentach.
		private int Value { get; }
		//
		public MultiplierBonus(int value, WealthCategory category) : base(category)
		{
			if (!ValidateValues(value, category, out string message))
				throw new ArgumentOutOfRangeException(message);
			Value = value;
		}
		// Bonus wykonuje się na odpowiednich tablicach.
		public override void Execute(int[] values, int[] multipliers, int fertility)
		{
			if (multipliers == null)
				throw new ArgumentNullException("multipliers");
			if (multipliers.Length != WealthCategoriesCount)
				throw new ArgumentException("Length of multipliers array should be equal to wealth categories count.", "multipliers");
			if (Category != WealthCategory.All)
				multipliers[(int)Category] += Value;
			else
				for (int whichCategory = 0; whichCategory < WealthCategoriesCount - 1; ++whichCategory)
					multipliers[whichCategory] += Value;
		}
		// Sprawdzenie poprawności wartości,
		public static bool ValidateValues(int value, WealthCategory category, out string message)
		{
			if (category == WealthCategory.Maintenance)
			{
				message = "Multiplier wealth bonus in 'Maintenance' category.";
				return false;
			}
			if (value < 0)
			{
				message = "Negative multiplier bonus.";
				return false;
			}
			message = "Values are chosen correcly";
			return true;
		}
	}
}
