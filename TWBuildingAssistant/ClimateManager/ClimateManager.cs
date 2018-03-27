using System;
using System.Linq;
using System.Xml.Linq;
namespace ClimateAndWeather
{
	/// <summary>
	/// Instancja tej klasy zarządza wszystkimi dostępnymi typami klimatu.
	/// </summary>
	public class ClimateManager
	{
		// Singleton:
		//
		private static ClimateManager HiddenSingleton { get; set; } = null;
		/// <summary>
		/// Odwołanie do tego jedynego obiektu zarządzającego wszystkimi klimatami.
		/// </summary>
		public static ClimateManager Singleton
		{
			get
			{
				if (HiddenSingleton == null)
						HiddenSingleton = new ClimateManager();
				return HiddenSingleton;
			}
		}
		/// <summary>
		/// Tworzy instancję zbioru religii dostępnej później poprzez właściwość Singleton. Nie wywoływać więcej niż raz.
		/// </summary>
		private ClimateManager()
		{
			XDocument sourceFile;
			sourceFile = XDocument.Load(_sourceFilename);
			_climates = (from XElement element in sourceFile.Root.Elements() select new Climate(element)).ToArray();
		}
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Weryfikuje czy dany tekst jest nazwą któregoś kilamtu.
		/// </summary>
		/// <param name="input">Tekst do weryfikacji.</param>
		public Climate Parse(string input)
		{
			if (input == null)
				return null;
			foreach (Climate climate in _climates)
			{
				if (climate.Name == input)
					return climate;
			}
			return null;
		}
		//
		// Stałe:
		//
		private const string _sourceFilename = "twa_climates.xml";
		//
		// Stan wewnętrzny:
		//
		private Climate[] _climates;
	}
}
