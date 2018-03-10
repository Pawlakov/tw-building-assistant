using System;
namespace Wealth
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
				throw new WealthException("Próbowano utworzyć bonus mnożący bez koniecznej wartości.");
			if (_category == BonusCategory.MAINTENANCE)
				throw new WealthException(String.Format("Próbowano utworzyć bonus mnożący dla kategorii {0}.", categoryText));
			_value = Convert.ToDouble(innerText);
		}
		/// <summary>
		/// Wykonuje bonus.
		/// </summary>
		/// <param name="values">Referencja do tablicy wartości prostych.</param>
		/// <param name="multipliers">Referencja do tablicy mnożników.</param>
		/// <param name="fertility">Poziom żyzności.</param>
		public override void Execute(int[] values, double[] multipliers, int fertility)
		{
			if (_category != BonusCategory.ALL)
				multipliers[(int)_category] += _value;
			else
				for (int whichCategory = 0; whichCategory < BonusCategoriesCount - 1; ++whichCategory)
					multipliers[whichCategory] += _value;
		}
		public override string ToString()
		{
			return String.Format("+{0}% wealth from {1}", _value, _category);
		}
	}
}
