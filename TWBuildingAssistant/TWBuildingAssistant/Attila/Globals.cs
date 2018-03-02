using System;
namespace TWAssistant
{
	namespace Attila
	{
		public enum Resource { NONE, IRON, LEAD, GEMSTONES, OLIVE, FUR, WINE, SILK, MARBLE, SALT, GOLD, DYE, LUMBER, SPICE };
		public enum BuildingType { TOWN, CENTERTOWN, CITY, CENTERCITY, COAST, RESOURCE, SPICE };
		public enum BonusCategory { ALL, AGRICULTURE, HUSBANDRY, CULTURE, INDUSTRY, COMMERCE, MARITIME_COMMERCE, SUBSISTENCE, MAINTENANCE }; // Maintenance HAS TO BE THE LAST ONE
		public enum Religion { STATE, LACHRI, GRECHRI, ARICHRI, EACHRI, ROMPAG, CELPAG, GERPAG, SLAPAG, SEMPAG, MANICH, ZORO, TENGRI, JUDA, MINO };
		public enum Climate { NORTH, MEDIUM, EAST, SOUTH };
		public enum Weather { EXTREME, BAD, NORMAL };
		public static class Globals
		{
			static public readonly Map map;
			static public readonly FactionsList factions;
			static public readonly Religion stateReligion;
			static public readonly Faction faction;
			static public readonly bool useLegacy;
			static public readonly int levelOfTechnology;
			static public readonly Weather worstCaseWeather;
			static public readonly int fertilityDrop;
			static public readonly int minimalOrder;
			static public readonly int minimalSanitation;
			//
			static Globals()
			{
				map = new Map("XMLs/twa_map.xml");
				factions = new FactionsList("XMLs/ModPack/twa_mpack_factions.xml");
				//
				Console.Clear();
				Console.WriteLine("Avaible religions: ");
				for (int whichReligion = 1; whichReligion < ReligionTypesCount; ++whichReligion)
					Console.WriteLine("{0}. {1}", whichReligion, (Religion)whichReligion);
				Console.Write("Pick a religion: ");
				stateReligion = (Religion)Convert.ToInt32(Console.ReadLine());
				//
				Console.Write("Pick level of technology (0-4): ");
				levelOfTechnology = Convert.ToInt32(Console.ReadLine());
				//
				Console.Write("Use legacy technologies? (if possible) true/false: ");
				useLegacy = Convert.ToBoolean(Console.ReadLine());
				//
				Console.WriteLine("List of factions:");
				factions.ShowList();
				//
				Console.Write("Pick a faction: ");
				faction = factions[Convert.ToInt32(Console.ReadLine())];
				//
				Console.Clear();
				Console.WriteLine("0. Extreme");
				Console.WriteLine("1. Bad");
				Console.WriteLine("2. Normal");
				Console.Write("Choose worst case weather: ");
				worstCaseWeather = (Weather)Enum.Parse(typeof(Weather), Console.ReadLine());
				//
				Console.Write("How many climate change events occured: ");
				fertilityDrop = Convert.ToInt32(Console.ReadLine());
				//
				Console.Write("Choose minimal public order: ");
				minimalOrder = Convert.ToInt32(Console.ReadLine());
				//
				Console.Write("Choose minimal sanitation: ");
				minimalSanitation = Convert.ToInt32(Console.ReadLine());
				//
				faction.Buildings.ApplyLimitations();
			}
			//
			public static int ResourceTypesCount
			{
				get { return 14; }
			}
			public static int BuildingTypesCount
			{
				get { return 7; }
			}
			public static int BonusCategoriesCount
			{
				get { return 9; }
			}
			public static int ReligionTypesCount
			{
				get { return 15; }
			}
			//
			public static int EnvOrder(ProvinceData province)
			{
				int result = faction.Order;
				switch (province.Climate)
				{
					case Climate.NORTH:
						switch (worstCaseWeather)
						{
							case Weather.EXTREME:
								result += -4;
								break;
							case Weather.BAD:
								result += -3;
								break;
							case Weather.NORMAL:
								result += -2;
								break;
						}
						break;
					case Climate.MEDIUM:
						switch (worstCaseWeather)
						{
							case Weather.EXTREME:
								result += -5;
								break;
							case Weather.BAD:
								result += -5;
								break;
							case Weather.NORMAL:
								result += 0;
								break;
						}
						break;
					case Climate.SOUTH:
						switch (worstCaseWeather)
						{
							case Weather.EXTREME:
								result += -4;
								break;
							case Weather.BAD:
								result += -3;
								break;
							case Weather.NORMAL:
								result += 0;
								break;
						}
						break;
					case Climate.EAST:
						switch (worstCaseWeather)
						{
							case Weather.EXTREME:
								result += -5;
								break;
							case Weather.BAD:
								result += -5;
								break;
							case Weather.NORMAL:
								result += 0;
								break;
						}
						break;
				}
				return result;
			}
			public static int EnvFood(ProvinceData province)
			{
				switch (province.Climate)
				{
					case Climate.NORTH:
						switch (worstCaseWeather)
						{
							case Weather.EXTREME:
								return -8;
							case Weather.BAD:
								return -4;
							case Weather.NORMAL:
								return -2;
						}
						break;
					case Climate.MEDIUM:
						switch (worstCaseWeather)
						{
							case Weather.EXTREME:
								return -3;
							case Weather.BAD:
								return -3;
							case Weather.NORMAL:
								return -1;
						}
						break;
					case Climate.SOUTH:
						switch (worstCaseWeather)
						{
							case Weather.EXTREME:
								return -3;
							case Weather.BAD:
								return -3;
							case Weather.NORMAL:
								return -1;
						}
						break;
					case Climate.EAST:
						switch (worstCaseWeather)
						{
							case Weather.EXTREME:
								return -3;
							case Weather.BAD:
								return -3;
							case Weather.NORMAL:
								return -1;
						}
						break;
				}
				return 0;
			}
			public static int EnvSanitation
			{
				get
				{
					if (stateReligion == Religion.ROMPAG)
						return faction.Sanitation;
					return faction.Sanitation - 2;
				}
			}
			public static int EnvOsmosis
			{
				get
				{
					if (stateReligion == Religion.LACHRI)
						return 1;
					return 0;
				}
			}
			public static int EnvInfluence
			{
				get
				{
					int result = faction.Influence;
					if (stateReligion == Religion.ARICHRI)
						result += 1;
					return result;
				}			}
			public static int EnvScience
			{
				get
				{
					return 0;
				}
			}
			public static int EnvGrowth
			{
				get
				{
					int result = faction.Growth;
					if (stateReligion == Religion.ZORO)
						result += 1;
					return result;
				}
			}
			public static int EnvFertility
			{
				get
				{
					return faction.Fertility - fertilityDrop;
				}
			}
		}
	}
}