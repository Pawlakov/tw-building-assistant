using System.Collections.Generic;
namespace TWAssistant
{
	namespace Attila
	{
		class ProvinceWealth
		{
			readonly int fertility;
			float[] multipliers;
			float[] values;
			readonly List<WealthBonus> bonuses;
			bool isCurrent;
			float wealth;
			//
			public ProvinceWealth(int iniFertility)
			{
				fertility = iniFertility;
				multipliers = new float[Globals.BonusCategoriesCount];
				values = new float[Globals.BonusCategoriesCount];
				bonuses = new List<WealthBonus>();
				isCurrent = false;
				wealth = 0;
				//
				if (Globals.faction.WealthBonuses != null)
					for (int whichBonus = 0; whichBonus < Globals.faction.WealthBonuses.Length; ++whichBonus)
						bonuses.Add(Globals.faction.WealthBonuses[whichBonus]);
			}
			public float Wealth
			{
				get
				{
					if (isCurrent)
					{
						return wealth;
					}
					wealth = 0;
					ExecuteBonuses();
					return wealth;
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
}