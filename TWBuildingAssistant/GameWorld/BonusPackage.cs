using System;
using System.Collections.Generic;
using GameWorld.WealthBonuses;

namespace GameWorld
{
	// Obiekty tej klasy służą do przechowania bonusów.
	public class BonusPackage
	{
		public int PublicOrder { get; set; }
		public int Food { get; set; }
		public int Sanitation { get; set; }
		public int ReligiousOsmosis { get; set; }
		public int ReligiousInfluence { get; set; }
		public int ResearchRate { get; set; }
		public int Growth { get; set; }
		public int Fertility { get; set; }
		public IEnumerable<WealthBonus> Bonuses { get; set; }
	}
}
