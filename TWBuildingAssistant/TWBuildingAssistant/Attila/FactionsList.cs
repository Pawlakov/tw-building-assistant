using System;
using System.Xml;
namespace TWAssistant
{
	namespace Attila
	{
		public class FactionsList
		{
			readonly Faction[] factions;
			//
			public FactionsList(string filename)
			{
				XmlDocument sourceFile = new XmlDocument();
				sourceFile.Load(filename);
				XmlNodeList factionNodesList = sourceFile.GetElementsByTagName("faction");
				factions = new Faction[factionNodesList.Count];
				for (int whichFaction = 0; whichFaction < factions.Length; ++whichFaction)
				{
					factions[whichFaction] = new Faction(factionNodesList[whichFaction]);
				}

			}
			//
			public Faction this[int whichFaction]
			{
				get { return factions[whichFaction]; }
			}
			//
			public void ShowList()
			{
				for (int whichFaction = 0; whichFaction < factions.Length; ++whichFaction)
					Console.WriteLine("{0}. {1}", whichFaction, factions[whichFaction].Name);
			}
		}
	}
}