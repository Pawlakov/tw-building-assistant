using System;
using System.Globalization;
namespace GameWorld.WealthBonuses
{
	// Bonus którego wartość zależy od poziomu żyzności.
	public class FertilityDependentBonus : WealthBonus
	{
		private int Value { get; }
		//
		public FertilityDependentBonus(int value, WealthCategory category) : base(category)
		{
			if (!ValidateValues(value, category, out string message))
				throw new ArgumentOutOfRangeException(message);
			Value = value;
		}
		// Dodanie bonusu do tablic z wartościami.
		public override void Execute(int[] values, int[] multipliers, int fertility)
		{
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length != WealthCategoriesCount)
				throw new ArgumentException("Length of values array should be equal to wealth categories count.", "values");
			if (fertility > 6 || fertility < 0)
				throw new ArgumentOutOfRangeException("fertility", fertility, "Fertility level out of range.");
			values[(int)Category] += (Value * fertility);
		}
		// Sprawdzenie poprawności wartości.
		public static bool ValidateValues(int value, WealthCategory category, out string message)
		{
			if (category != WealthCategory.Agriculture && category != WealthCategory.Husbandry)
			{
				message = "Fertility-dependent bonus outside of 'Agriculture' and 'Husbandry' categories.";
				return false;
			}
			if (value < 0)
			{
				message = "Negative fertility-dependent bonus.";
				return false;
			}
			message = "Values are chosen correcly";
			return true;
		}
	}
}
