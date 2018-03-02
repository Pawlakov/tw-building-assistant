using System;
using System.Xml;
namespace TWAssistant
{
	namespace Attila
	{
		public class WealthBonus
		{
			readonly BonusCategory category;
			readonly bool isMultiplier;
			readonly float value;
			readonly float perFertilityValue;
			//
			public WealthBonus(XmlNode wealthBonusNode)
			{
				category = (BonusCategory)Enum.Parse(typeof(BonusCategory), wealthBonusNode.Attributes.GetNamedItem("c").InnerText);
				//
				isMultiplier = Convert.ToBoolean(wealthBonusNode.Attributes.GetNamedItem("m").InnerText);
				if (category == BonusCategory.ALL && isMultiplier == false)
					throw new Exception("Non-percentage ALL bonus.");
				//
				string innerText = wealthBonusNode.InnerText;
				if (innerText.Contains("|"))
				{
					if ((category != BonusCategory.AGRICULTURE && category != BonusCategory.HUSBANDRY) || isMultiplier == true)
						throw new Exception("Wrong fertility-based bonus.");
					int divisionPosition = innerText.IndexOf('|');
					string firstValue = innerText.Substring(0, divisionPosition);
					string secondValue = innerText.Substring(divisionPosition + 1);
					value = Convert.ToSingle(firstValue);
					perFertilityValue = Convert.ToSingle(secondValue);
				}
				else
				{
					value = Convert.ToSingle(innerText);
					perFertilityValue = 0.0f;
				}
			}
			public WealthBonus(XmlNode wealthBonusNode, BonusCategory iniCategory)
			{
				category = iniCategory;
				//
				isMultiplier = Convert.ToBoolean(wealthBonusNode.Attributes.GetNamedItem("m").InnerText);
				if (category == BonusCategory.ALL && isMultiplier == false)
					throw new Exception("Non-percentage ALL bonus.");
				//
				string innerText = wealthBonusNode.InnerText;
				if (innerText.Contains("|"))
				{
					if ((category != BonusCategory.AGRICULTURE && category != BonusCategory.HUSBANDRY) || isMultiplier == true)
						throw new Exception("Wrong fertility-based bonus.");
					int divisionPosition = innerText.IndexOf('|');
					string firstValue = innerText.Substring(0, divisionPosition);
					string secondValue = innerText.Substring(divisionPosition + 1);
					value = Convert.ToSingle(firstValue);
					perFertilityValue = Convert.ToSingle(secondValue);
				}
				else
				{
					value = Convert.ToSingle(innerText);
					perFertilityValue = 0.0f;
				}
			}
			//
			public void Execute(ref float[] values, ref float[] multipliers, int fertility)
			{
				if (isMultiplier)
				{
					if (category != BonusCategory.ALL)
						multipliers[(int)category] += value;
					else
					{
						for (uint whichCategory = 0; whichCategory < Globals.BonusCategoriesCount - 1; ++whichCategory)
						{
							multipliers[whichCategory] += value;
						}
					}
				}
				else
				{
					values[(int)category] += (value + (perFertilityValue * fertility));
				}
			}
		}
	}
}