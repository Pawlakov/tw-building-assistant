using System;
using System.Xml;
namespace Wealth
{
	/// <summary>
	/// Dostępne kategorie bonusów. Pierwsza i ostatnia wartość (ALL i MAINTENANCE) są "specjalne".
	/// </summary>
	public enum BonusCategory
	{
		ALL,
		AGRICULTURE,
		HUSBANDRY,
		CULTURE,
		INDUSTRY,
		COMMERCE,
		MARITIME_COMMERCE,
		SUBSISTENCE,
		MAINTENANCE
	};
	/// <summary>
	/// Obiekty pochodnych tych klas reprezentują pojedyńcze bonusy dochodowe.
	/// </summary>
	public abstract class WealthBonus
	{
		/// <summary>
		/// Liczba dostępnych kategorii bonusów.
		/// </summary>
		public static int BonusCategoriesCount
		{
			get { return 9; }
		}
		/// <summary>
		/// Metoda tworząca obiekty SimpleBonus lub MultiplierBonus na podstawie węzła XML.
		/// </summary>
		/// <param name="node">Węzeł XML zawierający niezbędne dane.</param>
		/// <returns></returns>
		public static WealthBonus MakeBonus(XmlNode node)
		{
			if (node == null)
				throw new WealthException("Próbowano utworzyć bonus dochodowy na podstawie pustej wartości.");
			//
			XmlAttributeCollection attributes = node.Attributes;
			if (node.Name == "multiplier")
				return new MultiplierBonus(node.InnerText, attributes.GetNamedItem("c").InnerText);
			if (node.Name == "bonus")
			{
				if (attributes.GetNamedItem("m") == null)
					throw new WealthException("Napotkano na staromodny węzeł XML.");
				return new SimpleBonus(node.InnerText, attributes.GetNamedItem("c").InnerText);
			}
			//
			throw new WealthException(String.Format("Próbowano utworzyć bonus dochodowy na podstawie węzła o nazwie {0}, podczas gdy dozwolone nazwy to bonus i multiplier.", node.Name));
		}
		//
		//
		//
		protected readonly BonusCategory _category;
		protected WealthBonus(string categoryText)
		{
			if (categoryText == null)
				throw new WealthException("Próbowano utworzyć bonus dochodowy bez koniecznej wartości.");
			if (Enum.IsDefined(typeof(BonusCategory), categoryText))
				_category = (BonusCategory)Enum.Parse(typeof(BonusCategory), categoryText);
			else
				throw new WealthException("Niewłaściwa nazwa kategorii bonusu dochodowego.");
		}
		public abstract void Execute(int[] values, double[] multipliers, int fertility);
		public abstract override string ToString();
	}
	public class WealthException : Exception
	{
		public WealthException() : base("Bład w module dochodów.") { }
		public WealthException(string message) : base(message) { }
		public WealthException(string message, Exception innerException) : base(message, innerException) { }
	}
}