using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
namespace GameWorld.Factions
{
	// Klasa do zarządzania wszystkimi nacjami gry.
	public class FactionsManager
	{
		// Elementy XML przechowywane do celu leniwej inicjalizacji.
		readonly XElement[] _elements;
		readonly Faction[] _factions;
		public FactionsManager(Map.IReligionParser religionsParser, Map.IResourceParser resourcesParser)
		{
			XDocument sourceFile = XDocument.Load(_fileName);
			_elements = (from XElement element in sourceFile.Root.Elements() select element).ToArray();
			_factions = new Faction[_elements.Length];
			_religionsParser = religionsParser;
			_resourcesParser = resourcesParser;
		}
		private const string _fileName = "Factions\\twa_factions.xml";
		private readonly Map.IReligionParser _religionsParser;
		private readonly Map.IResourceParser _resourcesParser;
		// Indeks aktualnie wypbranej nacji.
		private int FactionIndex { get; set; } = -1;
		public int FactionsCount
		{
			get { return _elements.Length; }
		}
		public void ChangeFaction(int whichFaction)
		{
			FactionIndex = whichFaction;
		}
		// Aktualnie wybrana nacja.
		public Faction Faction
		{
			get
			{
				// Jeżeli obiekt nie został póki co stworzony to teraz powstanie.
				if (_factions[FactionIndex] == null)
					_factions[FactionIndex] = new Faction(_elements[FactionIndex], _resourcesParser, _religionsParser);
				return _factions[FactionIndex];
			}
		}
		//
		public int PublicOrder
		{
			get { return Faction.PublicOrder; }
		}
		public int Food
		{
			get { return Faction.Food; }
		}
		public int Sanitation
		{
			get { return Faction.Sanitation; }
		}
		public int ReligiousOsmosis
		{
			get { return Faction.ReligiousOsmosis; }
		}
		public int ReligiousInfluence
		{
			get { return Faction.ReligiousInfluence; }
		}
		public int ResearchRate
		{
			get { return Faction.ResearchRate; }
		}
		public int Growth
		{
			get { return Faction.Growth; }
		}
		public int Fertility
		{
			get { return Faction.Fertility; }
		}
		public IEnumerable<WealthBonuses.WealthBonus> Bonuses
		{
			get { return Faction.Bonuses; }
		}
		// Zwraca pary: indeks nacji i jeje nazwa.
		public IEnumerable<KeyValuePair<int, string>> AllFactionsNames
		{
			get
			{
				List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>(FactionsCount);
				for (int whichFaction = 0; whichFaction < FactionsCount; ++whichFaction)
					result.Add(new KeyValuePair<int, string>(whichFaction, (string)_elements[whichFaction].Attribute("n")));
				return result;
			}
		}
	}
}