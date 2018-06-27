using System.Collections.Generic;
namespace GameWorld.Combinations
{
	// Kalkulator do obliczania dochodu w prowincji.
	class ProvinceWealth
	{
		private readonly int _fertility;
		// Czy obliczenia są aktualne.
		private bool _isCurrent;
		private float _actualWealth;
		// Tabele gdzie każda pozycja odpowiada innej kategorii dochodu.
		private readonly int[] _multipliers;
		private readonly int[] _values;
		// Bonusy mające wpływ.
		private readonly List<WealthBonuses.WealthBonus> _bonuses;
		//
		public ProvinceWealth(int fertility)
		{
			_fertility = fertility;
			_multipliers = new int[WealthBonuses.WealthBonus.WealthCategoriesCount];
			_values = new int[WealthBonuses.WealthBonus.WealthCategoriesCount];
			_bonuses = new List<WealthBonuses.WealthBonus>();
			_isCurrent = false;
		}
		//
		// Podsumowanie.
		public float Wealth
		{
			get
			{
				if (!_isCurrent)
					ExecuteBonuses();
				return _actualWealth;
			}
		}
		// Dodanie kolejnego bonusu.
		public void AddBonus(WealthBonuses.WealthBonus bonus)
		{
			_bonuses.Add(bonus);
			_isCurrent = false;
		}
		// Można też dodać ich wiele na raz.
		public void AddBonuses(IEnumerable<WealthBonuses.WealthBonus> bonuses)
		{
			foreach (WealthBonuses.WealthBonus bonus in bonuses)
				_bonuses.Add(bonus);
			_isCurrent = false;
		}
		//
		// Przeliczenie dochodu przy obecnej liście bonusów.
		private void ExecuteBonuses()
		{
			_actualWealth = 0;
			// Inicjalizacja tabel
			for (int whichCategory = 0; whichCategory < WealthBonuses.WealthBonus.WealthCategoriesCount; ++whichCategory)
			{
				_multipliers[whichCategory] = 100;
				_values[whichCategory] = 0;
			}
			// Wykonanie bonusów
			for (int whichBonus = 0; whichBonus < _bonuses.Count; ++whichBonus)
				_bonuses[whichBonus].Execute(_values, _multipliers, _fertility);
			// Zliczenie tabel
			for (int whichCategory = 0; whichCategory < WealthBonuses.WealthBonus.WealthCategoriesCount; ++whichCategory)
				_actualWealth += (_multipliers[whichCategory] * _values[whichCategory]);
			_actualWealth *= 0.01f;
			_isCurrent = true;
		}
	}
}