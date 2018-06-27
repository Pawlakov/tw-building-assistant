using System;
using EnumsNET;
using System.Xml.Linq;
namespace GameWorld.WealthBonuses
{
	// Klasa do tworzenia bonusów dochodowych z elementów XML.
	public static class BonusFactory
	{
		// Metoda tworząca.
		public static WealthBonus MakeBonus(XElement element)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			if (!ValidateElement(element, out string message))
				throw new FormatException(message);
			//
			if (element.Name == "multiplier")
				return new MultiplierBonus((int)element, Enums.Parse<WealthCategory>((string)element.Attribute("c")));
			if (element.Name == "fertility_dependent")
				return new FertilityDependentBonus((int)element, Enums.Parse<WealthCategory>((string)element.Attribute("c")));
			return new SimpleBonus((int)element, Enums.Parse<WealthCategory>((string)element.Attribute("c")));
		}
		// Sprawdzenie czy element XML jest poprawnym opisem bonusu.
		public static bool ValidateElement(XElement element, out string message)
		{
			if (element.Name != "multiplier" && element.Name != "bonus" && element.Name != "fertility_dependent")
			{
				message = "XML element doesn't describe any wealth bonus.";
				return false;
			}
			if (element.Attribute("c") == null)
			{
				message = "XML element lacks 'c' attribute.";
				return false;
			}
			if (!Enums.TryParse((string)element.Attribute("c"), out WealthCategory insignificant))
			{
				message = "XML element's 'c' attribute is not a WealthCategory value.";
				return false;
			}
			if (!Int32.TryParse((string)element, out int dummy))
			{
				message = "XML element's content is not an integer value.";
				return false;
			}
			bool result;
			string submessage;
			if (element.Name == "multiplier")
				result = MultiplierBonus.ValidateValues((int)element, Enums.Parse<WealthCategory>((string)element.Attribute("c")), out submessage);
			else if (element.Name == "fertility_dependent")
				result = FertilityDependentBonus.ValidateValues((int)element, Enums.Parse<WealthCategory>((string)element.Attribute("c")), out submessage);
			else
				result = SimpleBonus.ValidateValues((int)element, Enums.Parse<WealthCategory>((string)element.Attribute("c")), out submessage);
			if(!result)
			{
				message = "Values describing WealthBonus are mismatched : " + submessage;
				return false;
			}
			message = "XML element is a valid representation of a wealth bonus.";
			return true;
		}
	}
}
