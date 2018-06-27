using System;
using System.Linq;
namespace GameWorld.Combinations
{
	public class Combination
	{
		// Prowincja będąca opisywana przez tą kombinację.
		public Map.ProvinceData Province { get; }
		// Miejsca na budynki.
		private readonly BuildingSlot[][] _slots;
		// Wyliczone poziowy higieny w regionach tej prowincji.
		private readonly int[] _sanitation;
		//
		public BuildingSlot[][] Slots
		{
			get
			{
				var result = new BuildingSlot[_slots.Length][];
				int whichSubarray = 0;
				foreach (var subarray in _slots)
				{
					result[whichSubarray] = subarray.ToArray();
					++whichSubarray;
				}
				return result;
			}
		}
		public int[] Sanitation
		{
			get { return _sanitation.ToArray(); }
		}
		public int Food { get; private set; }
		public int PublicOrder { get; private set; }
		public int ReligiousOsmosis { get; private set; }
		public int Fertility { get; private set; }
		public int ResearchRate { get; private set; }
		public int Growth { get; private set; }
		public double Wealth { get; private set; }
		//
		public Combination(Map.ProvinceData province)
		{
			Province = province;
			_slots = new BuildingSlot[3][];
			_sanitation = new int[3];
			for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
			{
				_slots[whichRegion] = new BuildingSlot[Province[whichRegion].SlotsCount];
				for (int whichSlot = 0; whichSlot < _slots[whichRegion].Length; ++whichSlot)
					_slots[whichRegion][whichSlot] = new BuildingSlot(ConcludeSlotType(Province[whichRegion], whichSlot));
			}
		}
		//
		// Przeliczenie wszystkich parametrów prowincji.
		public void Calculate(BonusPackage environment)
		{
			// Żyznośc musi być wyliczona wczśnie ponieważ pozostałe obliczenia od niej zależą.
			CalculateFertility(environment);
			// Wpływ z zewnątrz.
			Food = environment.Food;
			PublicOrder = environment.PublicOrder;
			for (int whichRegion = 0; whichRegion < _sanitation.Length; ++whichRegion)
				_sanitation[whichRegion] = environment.Sanitation;
			ReligiousOsmosis = environment.ReligiousOsmosis;
			ResearchRate = environment.ResearchRate;
			Growth = environment.Growth;
			// Tworzenie kalkulatorów pomocniczych.
			ProvinceWealth wealthCalculator = new ProvinceWealth(Fertility);
			wealthCalculator.AddBonuses(environment.Bonuses);
			ProvinceReligion religionCalculator = new ProvinceReligion(Province.Traditions);
			religionCalculator.AddInfluence(environment.ReligiousInfluence);
			// Uwzględnienie wpływu kombinacji.
			for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
				for (int whichSlot = 0; whichSlot < _slots[whichRegion].Length; ++whichSlot)
				{
					Buildings.BuildingLevel level = _slots[whichRegion][whichSlot].Level;
					//
					if (level != null)
					{
						Food += level.Food(Fertility);
						PublicOrder += level.PublicOrder;
						for (int whichSanitation = 0; whichSanitation < _sanitation.Length; ++whichSanitation)
							_sanitation[whichSanitation] += level.ProvincionalSanitation;
						_sanitation[whichRegion] += level.RegionalSanitation;
						ReligiousOsmosis += level.ReligiousOsmosis;
						ResearchRate += level.ResearchRate;
						Growth += level.Growth;
						//
						Religions.IReligion potentialReligion = level.ContainingBranch.Religion;
						if (potentialReligion != null)
							religionCalculator.AddInfluence(level.ReligiousInfluence, potentialReligion);
						wealthCalculator.AddBonuses(level.Bonuses);
					}
				}
			// Uwzględnienie wyników kalkulatorów.
			PublicOrder += religionCalculator.PublicOrder;
			Wealth = wealthCalculator.Wealth;
		}
		//
		// Wyliczanie poziomu żyzności w tej kombinacji.
		private void CalculateFertility(BonusPackage environment)
		{
			// Uwzględnienie wpływu z zewnątrz.
			Fertility = environment.Fertility;
			// Uwzględnienie wpływu kombinacji.
			foreach (BuildingSlot[] slotsRegion in _slots)
				foreach (BuildingSlot slot in slotsRegion)
					Fertility += (slot.Level != null ? slot.Level.Fertility : 0);
			// Wartości graniczne.
			if (Fertility > 6)
				Fertility = 6;
			if (Fertility < 0)
				Fertility = 0;
		}
		// Funkcja do wyznaczania typu slotu na podstawie potrzebnych informacji.
		private Buildings.SlotType ConcludeSlotType(Map.RegionData region, int whichSlot)
		{
			if (whichSlot == 0)
			{
				if (region.IsCity)
					return Buildings.SlotType.CityCenter;
				return Buildings.SlotType.TownCenter;
			}
			if (region.IsCoastal && whichSlot == 1)
				return Buildings.SlotType.Coast;
			if (region.IsCity)
				return Buildings.SlotType.City;
			return Buildings.SlotType.Town;
		}
	}
}