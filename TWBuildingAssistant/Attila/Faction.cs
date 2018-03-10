using System.Xml;
namespace TWAssistant
{
	namespace Attila
	{
		public class Faction
		{
			readonly string name;
			readonly BuildingLibrary buildings;
			readonly TechLevel[] techLevels;
			//
			public Faction(XmlNode factionNode)
			{
				name = factionNode.Attributes.GetNamedItem("n").InnerText;
				buildings = new BuildingLibrary(factionNode.Attributes.GetNamedItem("b").InnerText);
				//
				XmlNodeList techlevelNodeList = factionNode.ChildNodes;
				techLevels = new TechLevel[5];
				techLevels[0] = new TechLevel(techlevelNodeList[0], null);
				for (int whichLevel = 1; whichLevel < 5; ++whichLevel)
					techLevels[whichLevel] = new TechLevel(techlevelNodeList[whichLevel], techLevels[whichLevel - 1]);
			}
			//
			public string Name
			{
				get { return name; }
			}
			public int Sanitation
			{
				get { return techLevels[Globals.levelOfTechnology].Sanitation; }
			}
			public int Fertility
			{
				get { return techLevels[Globals.levelOfTechnology].Fertility; }
			}
			public int Growth
			{
				get { return techLevels[Globals.levelOfTechnology].Growth; }
			}
			public int Order
			{
				get{ return techLevels[Globals.levelOfTechnology].Order; }
			}
			public int Influence
			{
				get{ return techLevels[Globals.levelOfTechnology].Influence; }
			}
			public WealthBonus[] WealthBonuses
			{
				get { return techLevels[Globals.levelOfTechnology].WealthBonuses; }
			}
			public BuildingLibrary Buildings
			{
				get { return buildings; }
			}
			//
			public void EvaluateBuildings()
			{
				buildings.EvaluateBuildings();
			}
		}
	}
}