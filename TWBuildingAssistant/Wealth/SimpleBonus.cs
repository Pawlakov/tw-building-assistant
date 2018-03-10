using System;
namespace Wealth
{
	/// <summary>
	/// Obiekt tej klasy reprezentuje pojedyńczy prosty bonus dochodowy.
	/// </summary>
	class SimpleBonus : WealthBonus
	{
		readonly int _value;
		readonly int _perFertilityValue;
		//
		/// <summary>
		/// Tworzy nowy bonus prosty.
		/// </summary>
		/// <param name="innerText">Tekstowa reprezentacja wartości bonusu.</param>
		/// <param name="categoryText">Tekstowa reprezentacja kategorii bonusu.</param>
		public SimpleBonus(string innerText, string categoryText) : base(categoryText)
		{
			if (innerText == null)
				throw new WealthException("Próbowano utworzyć bonus mnożący bez koniecznej wartości.");
			if (_category == BonusCategory.ALL)
				throw new WealthException(String.Format("Próbowano utworzyć bonus prosty dla kategorii {0}.", categoryText));
			if (innerText.Contains("|"))
			{
				if (_category != BonusCategory.AGRICULTURE && _category != BonusCategory.HUSBANDRY)
					throw new WealthException("Próbowano utworzyć bonus zależny od żyzności poza kategoriamy rolniczymi.");
				int divisionPosition = innerText.IndexOf('|');
				string firstValue = innerText.Substring(0, divisionPosition);
				string secondValue = innerText.Substring(divisionPosition + 1);
				_value = Convert.ToInt32(firstValue);
				_perFertilityValue = Convert.ToInt32(secondValue);
			}
			else
			{
				_value = Convert.ToInt32(innerText);
				_perFertilityValue = 0;
			}
		}
		/// <summary>
		/// Wykonuje bonus.
		/// </summary>
		/// <param name="values">Referencja do tablicy wartości prostych.</param>
		/// <param name="multipliers">Referencja do tablicy mnożników.</param>
		/// <param name="fertility">Poziom żyzności.</param>
		public override void Execute(int[] values, double[] multipliers, int fertility)
		{
			values[(int)_category] += (_value + (_perFertilityValue * fertility));
		}
		public override string ToString()
		{
			return String.Format("+{0} wealth from {1}", _value, _category);
		}
	}
}
