using System;
namespace TWAssistant
{
	namespace Attila
	{
		public class ProvinceCombination
		{
			readonly ProvinceData province;
			readonly BuildingSlot[][] slots;
			//
			int fertility;
			//
			int food;
			int order;
			readonly int[] sanitations;
			int osmosis;
			int science;
			int growth;
			//
			float wealth;
			//
			bool isCurrent;
			//
			public ProvinceCombination(ProvinceData data, bool useResources)
			{
				province = data;
				slots = new BuildingSlot[3][];
				for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
				{
					slots[whichRegion] = new BuildingSlot[province[whichRegion].SlotsCount];
					for (int whichSlot = 0; whichSlot < slots[whichRegion].Length; ++whichSlot)
						slots[whichRegion][whichSlot] = new BuildingSlot(province[whichRegion], useResources, whichSlot);
				}
				//
				sanitations = new int[3];
				isCurrent = false;
			}
			public ProvinceCombination(ProvinceCombination source)
			{
				province = source.province;
				slots = new BuildingSlot[3][];
				for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
				{
					slots[whichRegion] = new BuildingSlot[source.slots[whichRegion].Length];
					for (int whichSlot = 0; whichSlot < source.slots[whichRegion].Length; ++whichSlot)
						slots[whichRegion][whichSlot] = new BuildingSlot(source.slots[whichRegion][whichSlot]);
				}
				//
				sanitations = new int[3];
				isCurrent = false;
			}
			//
			public void Fill(XorShift random, BuildingLibrary simulationLibrary)
			{
				for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
				{
					BuildingLibrary regionLibrary = new BuildingLibrary(simulationLibrary);
					for (int whichSlot = 0; whichSlot < slots[whichRegion].Length; ++whichSlot)
						if (slots[whichRegion][whichSlot].BuildingBranch != null)
							regionLibrary.Remove(slots[whichRegion][whichSlot].BuildingBranch);
					for (int whichBuilding = 0; whichBuilding < slots[whichRegion].Length; ++whichBuilding)
						slots[whichRegion][whichBuilding].Fill(random, regionLibrary, province[whichRegion]);
				}
				isCurrent = false;
			}
			public void ShowContent(bool includeIndexes)
			{
				/*Console.WriteLine("{0}", province.Name);
				for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
				{
					if (includeIndexes)
						Console.Write(" {0}. ", whichRegion);
					Console.WriteLine("{0}", province[whichRegion].Name);
					for (int whichSlot = 0; whichSlot < slots[whichRegion].Length; ++whichSlot)
					{
						if (includeIndexes)
							Console.Write("  {0}. ", whichSlot);
						Console.WriteLine(slots[whichRegion][whichSlot]);
					}
				}
				Console.WriteLine("Wea:{0} Foo:{1} Ord:{2} San:{3}/{4}/{5} Osm:{6} Fer:{7} ", Wealth, Food, Order, getSanitation(0), getSanitation(1), getSanitation(2), ReligiousOsmosis, Fertility);*/
				Console.Write(StringizationHelper(includeIndexes));
			}
			public override string ToString()
			{
				/*string result = province.Name;
				for (int whichRegion = 0; whichRegion < slots.Length; ++whichRegion)
				{
					result += province[whichRegion].Name;
					for (int whichSlot = 0; whichSlot < slots[whichRegion].Length; ++whichSlot)
						result += slots[whichRegion][whichSlot];
				}
				result += string.Format("Wea:{0} Foo:{1} Ord:{2} San:{3}/{4}/{5} Osm:{6} Fer:{7} ", Wealth, Food, Order, getSanitation(0), getSanitation(1), getSanitation(2), ReligiousOsmosis, Fertility);
				return result;*/
				return StringizationHelper(false);
			}
			string StringizationHelper(bool includeIndexes) // FINISH DIS!
			{
				string result = province.Name + '\n';
				for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
				{
					if (includeIndexes)
						result += (" " + whichRegion + ". \n");
					result += (province[whichRegion].Name + '\n');
					for (int whichSlot = 0; whichSlot < slots[whichRegion].Length; ++whichSlot)
					{
						if (includeIndexes)
							result += ("  " + whichSlot + ". \n");
						result += (slots[whichRegion][whichSlot].ToString() + '\n');
					}
				}
				result += string.Format("Wealth:{0} Food:{1}\n" +
										"Order:{2} Sanitation:{3}/{4}/{5}\n" +
										"Osmosis:{6} Fertility:{7}\n" +
										"Science:+{8}% Growth:{9} \n", Wealth, Food, Order, getSanitation(0), getSanitation(1), getSanitation(2), ReligiousOsmosis, Fertility, Science, Growth);
				return result;
			}
			public void RewardBuildings()
			{
				for (int whichRegion = 0; whichRegion < slots.Length; ++whichRegion)
					for (int whichSlot = 0; whichSlot < slots[whichRegion].Length; ++whichSlot)
						slots[whichRegion][whichSlot].Reward();
			}
			public void ForceBuildings(BuildingLibrary simulationLibrary)
			{
				while (true)
				{
					Console.Clear();
					ShowContent(true);
					Console.Write("Force building/level? (0-No/1-Yes): ");
					if (Convert.ToInt32(Console.ReadLine()) == 0)
						break;
					ForceBuilding(simulationLibrary);
				}
			}
			void ForceBuilding(BuildingLibrary simulationLibrary)
			{
				int whichRegion;
				int whichSlot;
				int choice;
				Console.Write("Region: ");
				whichRegion = Convert.ToInt32(Console.ReadLine());
				Console.Write("Slot: ");
				whichSlot = Convert.ToInt32(Console.ReadLine());
				Console.Write("0-Level / 1-Building: ");
				choice = Convert.ToInt32(Console.ReadLine());
				if (choice == 0)
				{
					Console.Write("Level: ");
					choice = Convert.ToInt32(Console.ReadLine());
					slots[whichRegion][whichSlot].Level = choice;
				}
				else
				{
					simulationLibrary.ShowListOneType(slots[whichRegion][whichSlot].Type);
					Console.Write("Building: ");
					choice = Convert.ToInt32(Console.ReadLine());
					slots[whichRegion][whichSlot].BuildingBranch = simulationLibrary.GetExactBuilding(slots[whichRegion][whichSlot].Type, choice);
				}
				isCurrent = false;
			}
			//
			public BuildingSlot this[int whichRegion, int whichSlot]
			{
				get { return slots[whichRegion][whichSlot]; }
			}
			public int Food
			{
				get
				{
					if (!isCurrent)
						HarvestBuildings();
					return food;
				}
			}
			public int Order
			{
				get
				{
					if (!isCurrent)
						HarvestBuildings();
					return order;
				}
			}
			public int getSanitation(uint whichRegion)
			{
				if (!isCurrent)
					HarvestBuildings();
				return sanitations[whichRegion];
			}
			public int ReligiousOsmosis
			{
				get
				{
					if (!isCurrent)
						HarvestBuildings();
					return osmosis;
				}
			}
			public int Fertility
			{
				get
				{
					if (!isCurrent)
						HarvestBuildings();
					return fertility;
				}
			}
			public int Science
			{
				get
				{
					if (!isCurrent)
						HarvestBuildings();
					return science;
				}
			}
			public int Growth
			{
				get
				{
					if (!isCurrent)
						HarvestBuildings();
					return growth;
				}
			}
			public float Wealth
			{
				get
				{
					if (!isCurrent)
						HarvestBuildings();
					return wealth;
				}
			}
			//
			void HarvestBuildings()
			{
				// Fertility has to be harvested earlier (other things depend on it).
				HarvestFertility();
				// Simple variables initialization.
				food = Globals.EnvFood(province);
				order = Globals.EnvOrder(province);
				for (int whichRegion = 0; whichRegion < sanitations.Length; ++whichRegion)
					sanitations[whichRegion] = Globals.EnvSanitation;
				osmosis = Globals.EnvOsmosis;
				science = Globals.EnvScience;
				growth = Globals.EnvGrowth;
				// Initialize calculators.
				ProvinceWealth wealthCalculator = new ProvinceWealth(fertility);
				ProvinceReligion religionCalculator = new ProvinceReligion(province.Traditions, Globals.stateReligion);
				// For-every-building loop.
				for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
				{
					for (int whichSlot = 0; whichSlot < slots[whichRegion].Length; ++whichSlot)
					{
						BuildingLevel level = slots[whichRegion][whichSlot].BuildingLevel;
						// Simple variables.
						food += level.GetFood(fertility);
						order += level.order;
						for (int whichSanitation = 0; whichSanitation < sanitations.Length; ++whichSanitation)
							sanitations[whichSanitation] += level.provincionalSanitation;
						sanitations[whichRegion] += level.regionalSanitation;
						osmosis += level.religiousOsmosis;
						science += level.science;
						growth += level.growth;
						// Calculators.
						Religion? potentialReligion = slots[whichRegion][whichSlot].BuildingBranch.religion;
						if (potentialReligion.HasValue)
							religionCalculator.AddInfluence(level.religiousInfluence, potentialReligion.Value);
						for (int whichBonus = 0; whichBonus < level.wealthBonuses.Length; ++whichBonus)
							wealthCalculator.AddBonus(level.wealthBonuses[whichBonus]);
					}
				}
				// Harvesing Calculators
				order += religionCalculator.Order;
				wealth = wealthCalculator.Wealth;
				isCurrent = true;
			}
			void HarvestFertility()
			{
				fertility = province.Fertility + Globals.EnvFertility;
				for (int whichRegion = 0; whichRegion < 3; ++whichRegion)
					for (int whichSlot = 0; whichSlot < slots[whichRegion].Length; ++whichSlot)
						fertility += slots[whichRegion][whichSlot].BuildingLevel.irigation;
				if (fertility > 6)
					fertility = 6;
				if (fertility < 0)
					fertility = 0;
			}
		}
	}
}