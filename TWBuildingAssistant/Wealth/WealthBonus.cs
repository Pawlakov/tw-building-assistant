using System;
using System.Xml;
using System.Globalization;
namespace WealthBonuses
{
	/// <summary>
	/// Dostępne kategorie bonusów dochodowych.
	/// </summary>
	public enum WealthCategory
	{
		/// <summary>
		/// Bonus dotyczący wszystkich kategorii. Używać jedynie jako bonus mnożący.
		/// </summary>
		All,
		/// <summary>
		/// Bonus w kategorii uprawy.
		/// </summary>
		Agriculture,
		/// <summary>
		/// Bonus w kategorii hodowli zwierząt.
		/// </summary>
		Husbandry,
		/// <summary>
		/// Bonus w kategorii kultury.
		/// </summary>
		Culture,
		/// <summary>
		/// Bonus w kategorii przemysłu.
		/// </summary>
		Industry,
		/// <summary>
		/// Bonus w kategorii handlu lokalnego.
		/// </summary>
		LocalCommerce,
		/// <summary>
		/// Bonus w kategorii handlu morskiego.
		/// </summary>
		MaritimeCommerce,
		/// <summary>
		/// Bonus w kategorii "istnienia".
		/// </summary>
		Subsistence,
		/// <summary>
		/// "Bonus" w kategorii utrzymania. Używać jedynie jako ujemny bonus prosty.
		/// </summary>
		Maintenance
	};
	/// <summary>
	/// Obiekty pochodnych tych klas reprezentują pojedyńcze bonusy dochodowe.
	/// </summary>
	public abstract class WealthBonus
	{
		/// <summary>
		/// Liczba dostępnych kategorii bonusów dochodowych.
		/// </summary>
		public static int WealthCategoriesCount
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
				throw new ArgumentNullException("node");
			XmlAttributeCollection attributes = node.Attributes;
			if (node.Name == "multiplier")
				return new MultiplierBonus(node.InnerText, attributes.GetNamedItem("c").InnerText);
			if (node.Name == "bonus")
			{
				if (attributes.GetNamedItem("m") == null)
					throw new FormatException("Given node uses obsolete format.");
				return new SimpleBonus(node.InnerText, attributes.GetNamedItem("c").InnerText);
			}
			throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Given node is neither 'multiplier', nor 'bonus' but '{0}' whcih is unacceptable.", node.Name), "node");
		}
		//
		//
		//
		/// <summary>
		/// Kategoria dochodu której dotyczy ten bonus.
		/// </summary>
		protected WealthCategory Category { get; }
		/// <summary>
		/// Tworzy instancję bonusu dochodowego. Do użycia przez klasy dziedziczące.
		/// </summary>
		/// <param name="categoryText">Tekst dający się skonwertować do wartości kategorii dochodu.</param>
		protected WealthBonus(string categoryText)
		{
			if (categoryText == null)
				throw new ArgumentNullException("categoryText");
			if (Enum.IsDefined(typeof(WealthCategory), categoryText))
				Category = (WealthCategory)Enum.Parse(typeof(WealthCategory), categoryText);
			else
				throw new ArgumentException("String value was not recognized as WealthCategory.", "categoryText");
		}
		/// <summary>
		/// Wykonuje ten bonus na podanych teblicach.
		/// </summary>
		/// <param name="values">Tablica prostych wartości dochodu dla wszystkich kategorii.</param>
		/// <param name="multipliers">Tablica mnożników dla wszystkich kategorii.</param>
		/// <param name="fertility">Pozoim żyzności prowincji dla której bonusy są stosowane.</param>
		public abstract void Execute(int[] values, double[] multipliers, int fertility);
		/// <summary>
		/// Zwraca prosty opis tego bonusu.
		/// </summary>
		public abstract override string ToString();
	}
}