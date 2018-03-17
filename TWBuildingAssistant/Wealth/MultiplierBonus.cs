using System;
using System.Globalization;
namespace WealthBonuses
{
	/// <summary>
	/// Obiekt tej klasy reprezentuje pojedyńczy bonus dochodowy będący mnożnikiem.
	/// </summary>
	class MultiplierBonus : WealthBonus
	{
		readonly double _value;
		//
		/// <summary>
		/// Tworzy nowy bonus mnożący.
		/// </summary>
		/// <param name="innerText">Tekstowa reprezentacja wartości bonusu.</param>
		/// <param name="categoryText">Tekstowa reprezentacja kategorii bonusu.</param>
		public MultiplierBonus(string innerText, string categoryText) : base(categoryText)
		{
			if (innerText == null)
				throw new ArgumentNullException("innerText");
			if (Category == WealthCategory.Maintenance)
				throw new ArgumentOutOfRangeException("categoryText", categoryText, "You cannot create multiplier wealth bonus in 'MAINTENANCE' category.");
			try
			{
				_value = Convert.ToDouble(innerText, CultureInfo.InvariantCulture);
			}
			catch(Exception exception)
			{
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not convert {0} to valid value.", innerText), exception);
			}
			if (_value < 0)
				throw new ArgumentOutOfRangeException("innerText", innerText, "You cannot create negative multiplier bonus.");
		}
		/// <summary>
		/// Wykonuje ten bonus na podanych teblicach.
		/// </summary>
		/// <param name="values">Tablica prostych wartości dochodu dla wszystkich kategorii.</param>
		/// <param name="multipliers">Tablica mnożników dla wszystkich kategorii.</param>
		/// <param name="fertility">Pozoim żyzności prowincji dla której bonusy są stosowane.</param>
		public override void Execute(int[] values, double[] multipliers, int fertility)
		{
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length != WealthCategoriesCount)
				throw new ArgumentException("Length of values array should be equal to wealth categories count.", "values");
			if (multipliers == null)
				throw new ArgumentNullException("multipliers");
			if (multipliers.Length != WealthCategoriesCount)
				throw new ArgumentException("Length of multipliers array should be equal to wealth categories count.", "multipliers");
			if (fertility > 6 || fertility < 0)
				throw new ArgumentOutOfRangeException("fertility", fertility, "Fertility level out of range.");
			if (Category != WealthCategory.All)
				multipliers[(int)Category] += _value;
			else
				for (int whichCategory = 0; whichCategory < WealthCategoriesCount - 1; ++whichCategory)
					multipliers[whichCategory] += _value;
		}
		/// <summary>
		/// Zwraca prosty opis tego bonusu.
		/// </summary>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "+{0}% wealth from {1}", _value, Category);
		}
	}
}
