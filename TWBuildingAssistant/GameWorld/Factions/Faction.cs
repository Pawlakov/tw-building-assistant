using System;
using System.Xml.Linq;
using System.Collections.Generic;
namespace GameWorld.Factions
{
	// Obiekt tej klasy przedstawia jedną nację z gry.
	public class Faction
	{
		public Faction(XElement element, Map.IResourceParser resourceParser, Map.IReligionParser religionParser)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			Name = (string)element.Attribute("n");
			TechLevels = Technologies.TechnologyTreeFactory.MakeTechnologyTree((string)element.Attribute("t"));
			Buildings = new Buildings.BuildingLibrary((string)element.Attribute("b"), TechLevels, resourceParser, religionParser);
		}
		// Nazwa
		public string Name { get; }
		public int Sanitation
		{
			get { return TechLevels.CurrentLevel.Sanitation; }
		}
		public int Fertility
		{
			get { return TechLevels.CurrentLevel.Fertility; }
		}
		public int Growth
		{
			get { return TechLevels.CurrentLevel.Growth; }
		}
		public int PublicOrder
		{
			get { return TechLevels.CurrentLevel.PublicOrder; }
		}
		public int ReligiousInfluence
		{
			get { return TechLevels.CurrentLevel.ReligiousInfluence; }
		}
		public int Food
		{
			get { return TechLevels.CurrentLevel.Food; }
		}
		public int ReligiousOsmosis
		{
			get { return TechLevels.CurrentLevel.ReligiousOsmosis; }
		}
		public int ResearchRate
		{
			get { return TechLevels.CurrentLevel.ResearchRate; }
		}
		public IEnumerable<Effects.WealthBonus> Bonuses
		{
			get { return TechLevels.CurrentLevel.Bonuses; }
		}
		// Metoda pozwala zmienić poziom dostępnej technologii.
		public void ChangeDesiredTechnologyLevel(int whichLevel, bool useLegacyTechnolgies)
		{
			TechLevels.ChangeDesiredTechnologyLevel(whichLevel, useLegacyTechnolgies);
		}
		// Powiązana z nacją pula budynków.
		public Buildings.BuildingLibrary Buildings { get; }
		// Powiązane drzewo technologii.
		private Technologies.ITechnologyTree TechLevels { get; }
	}
}