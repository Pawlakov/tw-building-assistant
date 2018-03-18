using System;
using System.Xml;
namespace Buildings
{
	public class Building
	{
		internal Building(XmlNode node)
		{
			isLegacy = null;
			//
			food = 0;
			irigation = 0;
			foodPerFertility = 0;
			//
			order = 0;
			growth = 0;
			science = 0;
			//
			regionalSanitation = 0;
			provincionalSanitation = 0;
			//
			religiousInfluence = 0;
			religiousOsmosis = 0;
			//
			Usefuliness = 0;
			//
			XmlNode temporary = levelNode.Attributes.GetNamedItem("n");
			name = temporary.InnerText;
			temporary = levelNode.Attributes.GetNamedItem("l");
			level = Convert.ToInt32(temporary.InnerText);
			temporary = levelNode.Attributes.GetNamedItem("lot");
			levelOfTechnology = Convert.ToInt32(temporary.InnerText);
			temporary = levelNode.Attributes.GetNamedItem("lcy");
			if (temporary != null)
				isLegacy = Convert.ToBoolean(temporary.InnerText);
			//
			temporary = levelNode.Attributes.GetNamedItem("f");
			if (temporary != null)
				food = Convert.ToInt32(temporary.InnerText);
			temporary = levelNode.Attributes.GetNamedItem("i");
			if (temporary != null)
				irigation = Convert.ToInt32(temporary.InnerText);
			temporary = levelNode.Attributes.GetNamedItem("_f");
			if (temporary != null)
				foodPerFertility = Convert.ToInt32(temporary.InnerText);
			//
			temporary = levelNode.Attributes.GetNamedItem("o");
			if (temporary != null)
				order = Convert.ToInt32(temporary.InnerText);
			//
			temporary = levelNode.Attributes.GetNamedItem("g");
			if (temporary != null)
				growth = Convert.ToInt32(temporary.InnerText);
			//
			temporary = levelNode.Attributes.GetNamedItem("sc");
			if (temporary != null)
				science = Convert.ToInt32(temporary.InnerText);
			XmlNodeList bonusNodeList = levelNode.ChildNodes;
			wealthBonuses = new WealthBonus[bonusNodeList.Count];
			for (int whichBonus = 0; whichBonus < wealthBonuses.Length; ++whichBonus)
			{
				wealthBonuses[whichBonus] = new WealthBonus(bonusNodeList.Item(whichBonus));
			}
			//
			temporary = levelNode.Attributes.GetNamedItem("s");
			if (temporary != null)
				regionalSanitation = Convert.ToInt32(temporary.InnerText);
			temporary = levelNode.Attributes.GetNamedItem("_s");
			if (temporary != null)
				provincionalSanitation = Convert.ToInt32(temporary.InnerText);
			//
			temporary = levelNode.Attributes.GetNamedItem("r");
			if (temporary != null)
				religiousInfluence = Convert.ToInt32(temporary.InnerText);
			temporary = levelNode.Attributes.GetNamedItem("ro");
			if (temporary != null)
				religiousOsmosis = Convert.ToInt32(temporary.InnerText);
		}
		//
		/// <summary>
		/// Nazwa tego budynku.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Poziom tego budynku wewnątrz gałęzi budynków.
		/// </summary>
		public int Level { get; }
		//
		public int Irrigation { get; }
		public int PublicOrder { get; }
		public int Growth { get; }
		public int ResearchRate { get; }
		public WealthBonuses.WealthBonus[] WealthBonuses { get; }
		public int RegionalSanitation { get; }
		public int ProvincionalSanitation { get; }
		public int ReligiousInfluence { get; }
		public int ReligiousOsmosis { get; }
		//
		private int Food { get; }
		private int FoodPerFertility { get; }
		/// <summary>
		/// Zwraca żywność zapewnianą przez ten budynek dla podanego poziomu żyzności.
		/// </summary>
		/// <param name="fertility">Poziom żyzności.</param>
		public int GetFood(int fertility)
		{
			return Food + fertility * FoodPerFertility;
		}
		//
		public int LevelOfTechnology { get; }
		public virtual bool IsAvailable()
		{
			if (levelOfTechnology > Globals.levelOfTechnology)
			{
				if (isLegacy == true)
					return true;
				return false;
			}
			else
			{
				if (isLegacy == null)
					return true;
				if (isLegacy == true)
				{
					if (levelOfTechnology == 0)
						return true;
					return Globals.useLegacy;
				}
				return !Globals.useLegacy;
			}
			// By to miało sens należy wpierw przetworzyć moduł technologiczny.
		}
	}
}