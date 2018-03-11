using System;
using System.Xml;
namespace Technology
{
	internal class TechnologyLevel
	{
		public TechnologyLevel(XmlNode node)
		{
			if (node == null)
				throw new TechnologyException("Próbowano utworzyć poziom technologii na podstawie pustej wartości.");
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
				throw new TechnologyException("Nie powiodło się konwertowanie którejś z wartości liczbowych przy tworzeniu obiektu poziomu technologii.", exception);
			}
			WealthBonuses = new Wealth.WealthBonus[bonusNodesList.Count];
			try
			{
				for (int whichBonus = 0; whichBonus < WealthBonuses.Length; ++whichBonus)
					WealthBonuses[whichBonus] = Wealth.WealthBonus.MakeBonus(bonusNodesList[whichBonus]);
			}
			catch(Exception exception)
			{
				throw new TechnologyException("Nie powiodło się tworzenie bonusów dochodowych wewnątrz poziomu technolgii.", exception);
			}
		}
		public int Sanitation { get; private set; }
		public int Fertility { get; private set; }
		public int Growth { get; private set; }
		public int PublicOrder { get; private set; }
		public int ReligiousInfluence { get; private set; }
		public Wealth.WealthBonus[] WealthBonuses { get; private set; }
		public void Cumulate(TechnologyLevel added)
		{
			Sanitation += added.Sanitation;
			Fertility += added.Fertility;
			Growth += added.Growth;
			PublicOrder += added.PublicOrder;
			ReligiousInfluence += added.ReligiousInfluence;
			Wealth.WealthBonus[] newArray = new Wealth.WealthBonus[WealthBonuses.Length + added.WealthBonuses.Length];
			try
			{
				WealthBonuses.CopyTo(newArray, 0);
				added.WealthBonuses.CopyTo(newArray, WealthBonuses.Length);
			}
			catch(Exception exception)
			{
				throw new TechnologyException("Nie powiodło się łączenie tablic bonusów poziomu technologii.", exception);
			}
			WealthBonuses = newArray;
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
			if (WealthBonuses.Length > 0)
				foreach (Wealth.WealthBonus bonus in WealthBonuses)
					result += ("\n  " + bonus.ToString());
			return result;
		}
	}
}