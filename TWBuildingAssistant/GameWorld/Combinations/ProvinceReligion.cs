using System;
namespace GameWorld.Combinations
{
	// Kalkulator wpływów religijnych w prowincji.
	public class ProvinceReligion
	{
		public ProvinceReligion(Map.ProvinceTraditions traditions)
		{
			Influence = traditions.StateReligionTradition();
			Counterinfluence = traditions.OtherReligionsTradition();
		}
		// Dodaje pewną ilość wpływu danej religii.
		public void AddInfluence(int ammount, Religions.IReligion religion)
		{
			if (religion.IsState)
				Influence += ammount;
			else
				Counterinfluence += ammount;
		}
		// Dodaje pewną ilość wpływu religii państwowej.
		public void AddInfluence(int ammount)
		{
			Influence += ammount;
		}
		// Kara do porządku publicznego przy obecnych proporcjach wpływów.
		public int PublicOrder
		{
			get
			{
				// Przybliżenie funkcji używanej przez grę.
				float percentage = StateReligionPercentage;
				return (int)Math.Floor(-6 + (percentage * 17 + 220) / 300);
			}
		}
		// Wpływ religii państwowej.
		public int Influence { get; private set; }
		// Pozostały wpływ.
		public int Counterinfluence { get; private set; }
		public float StateReligionPercentage
		{
			get
			{
				return 100 * (Influence / (Counterinfluence + (float)Influence));
			}
		}
	}
}
