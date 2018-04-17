using System;
using System.Xml;
namespace TWAssistant
{
	namespace Attila
	{
		public class TechLevel
		{
			readonly TechLevel previous;
			int[] sanitation;
			int[] fertility;
			int[] growth;
			int[] order;
			int[] influence;
			readonly WealthBonus[][] wealthBonuses;
			//
			public TechLevel(XmlNode techlevelNode, TechLevel previousLevel)
			{
				previous = previousLevel;
				XmlNode universalNode = techlevelNode.ChildNodes[0];
				XmlNode antilegacyNode = techlevelNode.ChildNodes[1];
				//
				sanitation = new int[2];
				fertility = new int[2];
				growth = new int[2];
				order = new int[2];
				influence = new int[2];
				wealthBonuses = new WealthBonus[2][];
				for (int whichHalf = 0; whichHalf < 2; ++whichHalf)
				{
					sanitation[whichHalf] = 0;
					fertility[whichHalf] = 0;
					growth[whichHalf] = 0;
					order[whichHalf] = 0;
					influence[whichHalf] = 0;
					wealthBonuses[whichHalf] = new WealthBonus[0];
				}
				//
				Halfinitializer(universalNode, 0);
				Halfinitializer(antilegacyNode, 1);
				SumUpArrays();
			}
			void Halfinitializer(XmlNode halfNode, int whichHalf)
			{
				XmlNode temporary;
				temporary = halfNode.Attributes.GetNamedItem("s");
				if (temporary != null)
					sanitation[whichHalf] = Convert.ToInt32(temporary.InnerText);
				//
				temporary = halfNode.Attributes.GetNamedItem("i");
				if (temporary != null)
					fertility[whichHalf] = Convert.ToInt32(temporary.InnerText);
				//
				temporary = halfNode.Attributes.GetNamedItem("g");
				if (temporary != null)
					growth[whichHalf] = Convert.ToInt32(temporary.InnerText);
				//
				temporary = halfNode.Attributes.GetNamedItem("o");
				if (temporary != null)
					order[whichHalf] = Convert.ToInt32(temporary.InnerText);
				//
				temporary = halfNode.Attributes.GetNamedItem("r");
				if (temporary != null)
					influence[whichHalf] = Convert.ToInt32(temporary.InnerText);
				//
				XmlNodeList bonusNodesList = halfNode.ChildNodes;
				wealthBonuses[whichHalf] = new WealthBonus[bonusNodesList.Count];
				for (int whichBonus = 0; whichBonus < wealthBonuses[whichHalf].Length; ++whichBonus)
					wealthBonuses[whichHalf][whichBonus] = new WealthBonus(bonusNodesList[whichBonus]);
			}
			void SumUpArrays()
			{
				WealthBonus[] newArray;
				//
				sanitation[1] += sanitation[0];
				fertility[1] += fertility[0];
				growth[1] += growth[0];
				order[1] += order[0];
				influence[1] += influence[0];
				newArray = new WealthBonus[wealthBonuses[0].Length + wealthBonuses[1].Length];
				wealthBonuses[0].CopyTo(newArray, 0);
				wealthBonuses[1].CopyTo(newArray, wealthBonuses[0].Length);
				wealthBonuses[1] = newArray;
				//
				if (previous != null)
					for (int whichHalf = 0; whichHalf < 2; ++whichHalf)
					{
						sanitation[whichHalf] += previous.sanitation[whichHalf];
						fertility[whichHalf] += previous.fertility[whichHalf];
						growth[whichHalf] += previous.growth[whichHalf];
						order[whichHalf] += previous.order[whichHalf];
						influence[whichHalf] += previous.influence[whichHalf];
						newArray = new WealthBonus[wealthBonuses[whichHalf].Length + previous.wealthBonuses[whichHalf].Length];
						wealthBonuses[whichHalf].CopyTo(newArray, 0);
						previous.wealthBonuses[whichHalf].CopyTo(newArray, wealthBonuses[whichHalf].Length);
						wealthBonuses[whichHalf] = newArray;
					}
			}
			//
			public int Sanitation
			{
				get
				{
					if (Globals.useLegacy)
						return sanitation[0];
					return sanitation[1];
				}
			}
			public int Fertility
			{
				get
				{
					if (Globals.useLegacy)
						return fertility[0];
					return fertility[1];
				}
			}
			public int Growth
			{
				get
				{
					if (Globals.useLegacy)
						return growth[0];
					return growth[1];
				}
			}
			public int Order
			{
				get
				{
					if (Globals.useLegacy)
						return order[0];
					return order[1];
				}
			}
			public int Influence
			{
				get
				{
					if (Globals.useLegacy)
						return influence[0];
					return influence[1];
				}
			}
			public WealthBonus[] WealthBonuses
			{
				get
				{
					if (Globals.useLegacy)
						return wealthBonuses[0];
					return wealthBonuses[1];
				}
			}
		}
	}
}