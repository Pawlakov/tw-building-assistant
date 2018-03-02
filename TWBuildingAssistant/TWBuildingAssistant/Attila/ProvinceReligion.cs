using System;
namespace TWAssistant
{
	namespace Attila
	{
		public class ProvinceReligion
		{
			int influence;
			int counterinfluence;
			Religion religion;
			//
			public ProvinceReligion(ProvinceTraditions traditions, Religion religion)
			{
				this.religion = religion;
				influence = traditions.GetTradionExactly(religion) + Globals.EnvInfluence + 0; // 0 is for incoming osmosis.
				counterinfluence = traditions.GetTraditionExcept(religion);
			}
			public void AddInfluence(int ammount, Religion religion)
			{
				if (this.religion == religion || religion == Religion.STATE)
					influence += ammount;
				else
					counterinfluence += ammount;
			}
			public int Order
			{
				get
				{
					float percentage = StateReligionPercentage;
					return (int)Math.Floor(-6 + (percentage * 17 + 220) / 300);
				}
			}
			public int Influence
			{
				get { return influence; }
			}
			public int Counterinfluence
			{
				get { return counterinfluence; }
			}
			public float StateReligionPercentage
			{
				get
				{
					return 100 * (influence / (counterinfluence + (float)influence));
				}
			}
		}
	}
}
