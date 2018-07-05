using System;
using System.Linq;
using System.Collections.Generic;

namespace GameWorld
{
	// Cały świat gry używany do symulacji.
	public class World
	{
		private readonly Resources.ResourcesManager _resourcesManager;
		private readonly Religions.ReligionsManager _religionsManager;
		private readonly Map.ProvincesManager _provincesManager;
		private readonly Factions.FactionsManager _factionsManager;
		//
		public World()
		{
			_resourcesManager = new Resources.ResourcesManager();
			_religionsManager = new Religions.ReligionsManager();
			_provincesManager = new Map.ProvincesManager(_religionsManager, _resourcesManager, _religionsManager);
			_factionsManager = new Factions.FactionsManager(_religionsManager, _resourcesManager);
		}
		// Metoda generuje obiekt na którym odbywać się będzie symulacja dla zadanych ustawień.
		public SimulationKit AssembleSimulationKit(WorldSettings settings)
		{
			_religionsManager.ChangeStateReligion(settings.StateReligionIndex);
			//
			_provincesManager.ChangeFertilityDrop(settings.FertilityDrop);
			_provincesManager.ChangeProvince(settings.ProvinceIndex);
			_provincesManager.ChangeWorstCaseWeather(settings.WorstCaseWeather);
			//
			_factionsManager.ChangeFaction(settings.FactionIndex);
			_factionsManager.Faction.ChangeDesiredTechnologyLevel(settings.DesiredTechnologyLevelIndex, settings.UseLegacyTechnologies);
			//
			Combinations.Combination combination = new Combinations.Combination(_provincesManager.Province);
			Buildings.BuildingLibrary pool = _factionsManager.Faction.Buildings;
			//
			return new SimulationKit(this, pool, combination);
		}
		public IEnumerable<KeyValuePair<int, string>> Religions
		{
			get
			{
				return _religionsManager.AllReligionsNames;
			}
		}
		public IEnumerable<KeyValuePair<int, string>> Provinces
		{
			get
			{
				return _provincesManager.AllProvincesNames;
			}
		}
		public IEnumerable<KeyValuePair<int, string>> Factions
		{
			get
			{
				return _factionsManager.AllFactionsNames;
			}
		}
		// Uchwytuje migawkę wpływu otoczenia na kombinację w tej chwili.
		public Effects.ProvincionalEffectsPackage Environment
		{
			get
			{
				return new Effects.ProvincionalEffectsPackage(
				_religionsManager.PublicOrder + _provincesManager.PublicOrder + _factionsManager.PublicOrder,
				_religionsManager.Food + _provincesManager.Food + _factionsManager.Food,
				_religionsManager.Sanitation + _provincesManager.Sanitation + _factionsManager.Sanitation,
				_religionsManager.ReligiousOsmosis + _provincesManager.ReligiousOsmosis +
				_factionsManager.ReligiousOsmosis,
				_religionsManager.ReligiousInfluence + _provincesManager.ReligiousInfluence +
				_factionsManager.ReligiousInfluence,
				_religionsManager.ResearchRate + _provincesManager.ResearchRate + _factionsManager.ResearchRate,
				_religionsManager.Growth + _provincesManager.Growth + _factionsManager.Growth,
				_religionsManager.Fertility + _provincesManager.Fertility + _factionsManager.Fertility,
				_religionsManager.Bonuses.Concat(_provincesManager.Bonuses.Concat(_factionsManager.Bonuses)));
			}
		}
	}
	// Paczka ustawień dla świata.
	public class WorldSettings
	{
		public int StateReligionIndex { get; set; }
		public int FertilityDrop { get; set; }
		public ClimateAndWeather.Weather WorstCaseWeather { get; set; }
		public int FactionIndex { get; set; }
		public int DesiredTechnologyLevelIndex { get; set; }
		public bool UseLegacyTechnologies { get; set; }
		public int ProvinceIndex { get; set; }
	}
}