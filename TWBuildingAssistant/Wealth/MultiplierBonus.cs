using System;
using System.Globalization;
namespace WealthBonuses
{
	/// <summary>
	/// Obiekt tej klasy reprezentuje pojedyńczy bonus dochodowy będący mnożnikiem.
	/// </summary>
	public class MultiplierBonus : WealthBonus
	{
		// Stan wewnętrzny:
		//
		private double Value { get; }
		//
		// Interfejs wewnętrzny:
		//
		internal MultiplierBonus(string innerText, string categoryText) : base(categoryText)
		{
			if (innerText == null)
				throw new ArgumentNullException("innerText");
			if (Category == WealthCategory.Maintenance)
				throw new ArgumentOutOfRangeException("categoryText", categoryText, "You cannot create multiplier wealth bonus in 'MAINTENANCE' category.");
			try
			{
				Value = Convert.ToDouble(innerText, CultureInfo.InvariantCulture);
			}
			catch(Exception exception)
			{
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not convert {0} to valid value.", innerText), exception);
			}
			if (Value < 0)
				throw new ArgumentOutOfRangeException("innerText", innerText, "You cannot create negative multiplier bonus.");
		}
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Wykonuje ten bonus na podanych teblicach.
		/// </summary>
		/// <param name="values">Tablica prostych wartości dochodu dla wszystkich kategorii.</param>
		/// <param name="multipliers">Tablica mnożników dla wszystkich kategorii.</param>
		/// <param name="fertility">Pozoim żyzności prowincji dla której bonusy są stosowane.</param>
		public override void Execute(int[] values, double[] multipliers, int fertility)
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
		/// <summary>
		/// Zwraca prosty opis tego bonusu.
		/// </summary>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "+{0}% wealth from {1}", Value, Category);
		}
	}
}
