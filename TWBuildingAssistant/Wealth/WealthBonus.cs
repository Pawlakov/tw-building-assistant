using System;
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
		// Stan wewnętrzny:
		//
		/// <summary>
		/// Kategoria dochodu której dotyczy ten bonus.
		/// </summary>
		protected WealthCategory Category { get; }
		//
		// Interfejs wewnętrzny:
		//
		internal int WealthCategoriesCount
		{
			get { return _wealthCategoriesCount; }
		}
		//
		// Stałe:
		//
		private const int _wealthCategoriesCount = 9;
		//
		// Interfejs publiczny:
		//
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
		//
		// Pomocnicze:
		//
		/// <summary>
		/// Tworzy instancję bonusu dochodowego. Do użycia przez klasy dziedziczące.
		/// </summary>
		/// <param name="categoryText">Tekst dający się skonwertować do wartości kategorii dochodu.</param>
		protected WealthBonus(string categoryText)
		{
			if (Enum.IsDefined(typeof(WealthCategory), categoryText))
				Category = (WealthCategory)Enum.Parse(typeof(WealthCategory), categoryText);
			else
				throw new ArgumentException("String value was not recognized as any wealth category.", "categoryText");
		}
	}
}