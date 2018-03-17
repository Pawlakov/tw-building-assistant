using System.Collections.Generic;
namespace Wealth
{
	public class ProvinceWealth
	{
		readonly int _fertility;
		double[] _multipliers;
		int[] _values;
		readonly List<WealthBonus> _bonuses;
		bool _isCurrent;
		float _wealth;
		//
		public ProvinceWealth(int fertility)
		{
			_fertility = fertility;
			_multipliers = new double[WealthBonus.BonusCategoriesCount];
			_values = new int[WealthBonus.BonusCategoriesCount];
			_bonuses = new List<WealthBonus>();
			_isCurrent = false;
			if (Globals.faction.WealthBonuses != null)
				for (int whichBonus = 0; whichBonus < Globals.faction.WealthBonuses.Length; ++whichBonus)
					bonuses.Add(Globals.faction.WealthBonuses[whichBonus]);
		}
		public float Wealth
		{
			get
			{
				if (_isCurrent)
					return _wealth;
				ExecuteBonuses();
				return _wealth;
			}
		}
		//
		public void AddBonus(WealthBonus bonus)
		{
			bonuses.Add(bonus);
			isCurrent = false;
		}
		//
		void ExecuteBonuses()
		{
			_wealth = 0;
			for (int whichCategory = 0; whichCategory < Globals.BonusCategoriesCount; ++whichCategory)
			{
				multipliers[whichCategory] = 1;
				values[whichCategory] = 0;
			}
			for (int whichBonus = 0; whichBonus < bonuses.Count; ++whichBonus)
				bonuses[whichBonus].Execute(ref values, ref multipliers, fertility);
			for (int whichCategory = 0; whichCategory < Globals.BonusCategoriesCount; ++whichCategory)
				wealth += (multipliers[whichCategory] * values[whichCategory]);
			isCurrent = true;
		}
	}
}