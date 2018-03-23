using System;
using System.Xml;
namespace Faction
{
	public class TechnologyLevel
	{
		public TechnologyLevel(TechnologyTree containingTree, XmlNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			if (containingTree == null)
				throw new ArgumentNullException("containingTree");
			if (node.Name != "techlevel")
				throw new ArgumentException("Given node is not technology level node.", "node");
			ContainingTree = containingTree;
			XmlNodeList bonusNodesList = node.ChildNodes;
			Sanitation = 0;
			Fertility = 0;
			Growth = 0;
			PublicOrder = 0;
			ReligiousInfluence = 0;
			XmlNode temporary;
			try
			{
				temporary = node.Attributes.GetNamedItem("s");
				if (temporary != null)
					Sanitation = Convert.ToInt32(temporary.InnerText);
				temporary = node.Attributes.GetNamedItem("i");
				if (temporary != null)
					Fertility = Convert.ToInt32(temporary.InnerText);
				temporary = node.Attributes.GetNamedItem("g");
				if (temporary != null)
					Growth = Convert.ToInt32(temporary.InnerText);
				temporary = node.Attributes.GetNamedItem("po");
				if (temporary != null)
					PublicOrder = Convert.ToInt32(temporary.InnerText);
				temporary = node.Attributes.GetNamedItem("ri");
				if (temporary != null)
					ReligiousInfluence = Convert.ToInt32(temporary.InnerText);
			}
			catch(Exception exception)
			{
				throw new FormatException("Failed to load attributes of this technology level.", exception);
			}
			Bonuses = new WealthBonuses.WealthBonus[bonusNodesList.Count];
			try
			{
				for (int whichBonus = 0; whichBonus < Bonuses.Length; ++whichBonus)
					Bonuses[whichBonus] = WealthBonuses.WealthBonus.MakeBonus(bonusNodesList[whichBonus]);
			}
			catch(Exception exception)
			{
				throw new Exception("Failed to create wealth bonus objects for this technology level.", exception);
			}
		}
		private TechnologyTree ContainingTree { get; }
		public int Sanitation { get; private set; }
		public int Fertility { get; private set; }
		public int Growth { get; private set; }
		public int PublicOrder { get; private set; }
		public int ReligiousInfluence { get; private set; }
		public WealthBonuses.WealthBonus[] Bonuses { get; private set; }
		public void Cumulate(TechnologyLevel added)
		{
			if (added == null)
				throw new ArgumentNullException("added");
			Sanitation += added.Sanitation;
			Fertility += added.Fertility;
			Growth += added.Growth;
			PublicOrder += added.PublicOrder;
			ReligiousInfluence += added.ReligiousInfluence;
			WealthBonuses.WealthBonus[] newArray = new WealthBonuses.WealthBonus[Bonuses.Length + added.Bonuses.Length];
			try
			{
				Bonuses.CopyTo(newArray, 0);
				added.Bonuses.CopyTo(newArray, Bonuses.Length);
			}
			catch(Exception exception)
			{
				throw new Exception("Failed to cumulate wealth bonuses of technology levels..", exception);
			}
			Bonuses = newArray;
		}
		public override string ToString()
		{
			string result = "";
			if (PublicOrder != 0)
				result += (" public order +" + PublicOrder);
			if (Growth != 0)
				result += (" growth +" + Growth);
			if (Fertility != 0)
				result += (" fertility +" + Growth);
			if (Sanitation != 0)
				result += (" sanitation +" + Sanitation);
			if (ReligiousInfluence != 0)
				result += (" religious influence +" + ReligiousInfluence);
			if (Bonuses.Length > 0)
				foreach (WealthBonuses.WealthBonus bonus in Bonuses)
					result += ("\n  " + bonus.ToString());
			return result;
		}
		public bool IsAvailable()
		{
			return ContainingTree(this);
		}
	}
}