using System;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
namespace WealthBonuses
{
	/// <summary>
	/// Statyczna klasa służąca do tworzenia obiektów bonusów dochodowych.
	/// </summary>
	public static class BonusFactory
	{
		// Fabryka obiektów:
		//
		/// <summary>
		/// Metoda tworząca obiekty SimpleBonus lub MultiplierBonus na podstawie elementu XML.
		/// </summary>
		/// <param name="element">Element XML zawierający niezbędne dane.</param>
		public static WealthBonus MakeBonus(XElement element)
		{
			if (element == null)
				throw new ArgumentNullException("node");
			if (element.Name == "multiplier")
				return new MultiplierBonus((string)element, (string)element.Attribute("c"));
			if (element.Name == "bonus")
				return new SimpleBonus((string)element, (string)element.Attribute("c"));
			throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Given XML element is neither multiplier, nor bonus but {0}.", element.Name), "element");
		}
	}
}
