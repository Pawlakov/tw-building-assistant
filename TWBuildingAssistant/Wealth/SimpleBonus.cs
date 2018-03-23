using System;
using System.Globalization;
namespace WealthBonuses
{
	/// <summary>
	/// Obiekt tej klasy reprezentuje pojedyńczy prosty bonus dochodowy.
	/// </summary>
	public class SimpleBonus : WealthBonus
	{
		// Stan wewnętrzny:
		//
		private int Value { get; }
		private int ValuePerFertility { get; }
		//
		// Interfejs wewnętrzny:
		//
		internal SimpleBonus(string innerText, string categoryText) : base(categoryText)
		{
			if (innerText == null)
				throw new ArgumentNullException("innerText");
			if (Category == WealthCategory.All)
				throw new ArgumentOutOfRangeException("categoryText", categoryText, "You cannot create simple wealth bonus in 'ALL' category.");
			string firstValue;
			string secondValue;
			if (innerText.Contains("|"))
			{
				if (Category != WealthCategory.Agriculture && Category != WealthCategory.Husbandry)
					throw new ArgumentOutOfRangeException("innerText", innerText, "You cannot create fertility-dependent bonus in this category.");
				int divisionPosition = innerText.IndexOf('|');
				firstValue = innerText.Substring(0, divisionPosition);
				secondValue = innerText.Substring(divisionPosition + 1);
			}
			else
			{
				firstValue = innerText;
				secondValue = "0";
			}
			try
			{
				Value = Convert.ToInt32(firstValue, CultureInfo.InvariantCulture);
				ValuePerFertility = Convert.ToInt32(secondValue, CultureInfo.InvariantCulture);
			}
			catch (Exception exception)
			{
				throw new FormatException(String.Format(CultureInfo.CurrentCulture, "Could not convert {0} to valid value(s).", innerText), exception);
			}
			if (ValuePerFertility < 0)
				throw new ArgumentOutOfRangeException("innerText", innerText, "You cannot create negative fertility-dependent bonus.");
			if (Category == WealthCategory.Maintenance && Value > 0)
				throw new ArgumentOutOfRangeException("innerText", innerText, "You cannot create positive maintenance bonus.");
			if (Category != WealthCategory.Maintenance && Value < 0)
				throw new ArgumentOutOfRangeException("innerText", innerText, "You cannot create negative non-maintenance bonus.");
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
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length != WealthCategoriesCount)
				throw new ArgumentException("Length of values array should be equal to wealth categories count.", "values");
			if (fertility > 6 || fertility < 0)
				throw new ArgumentOutOfRangeException("fertility", fertility, "Fertility level out of range.");
			values[(int)Category] += (Value + (ValuePerFertility * fertility));
		}
		/// <summary>
		/// Zwraca prosty opis tego bonusu.
		/// </summary>
		public override string ToString()
		{
			if(ValuePerFertility == 0)
				return String.Format(CultureInfo.CurrentCulture, "+{0} wealth from {1}", Value, Category);
			return String.Format(CultureInfo.CurrentCulture, "+{0} (additionaly +{1} per every fertility level) wealth from {2}", Value, ValuePerFertility, Category);
		}
	}
}
