using System;
using System.Xml;
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
		/// Metoda tworząca obiekty SimpleBonus lub MultiplierBonus na podstawie węzła XML.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający niezbędne dane.</param>
		public static WealthBonus MakeBonus(XmlNode node)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			XmlAttributeCollection attributes = node.Attributes;
			if (node.Name == "multiplier")
				return new MultiplierBonus(node.InnerText, attributes.GetNamedItem("c").InnerText);
			if (node.Name == "bonus")
			{
				if (attributes.GetNamedItem("m") != null)
					throw new FormatException("Given node uses obsolete format.");
				return new SimpleBonus(node.InnerText, attributes.GetNamedItem("c").InnerText);
			}
			throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Given node is neither multiplier, nor bonus but {0} which is unacceptable.", node.Name), "node");
		}
	}
}
