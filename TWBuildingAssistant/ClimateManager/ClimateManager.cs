using System;
using System.Xml;
using System.Globalization;
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
				{
					try
					{
						HiddenSingleton = new ClimateManager();
					}
					catch(Exception exception)
					{
						throw new Exception("Failed to create instance of climate manager.", exception);
					}
				}
				return HiddenSingleton;
			}
		}
		/// <summary>
		/// Tworzy instancję zbioru religii dostępnej później poprzez właściwość Singleton. Nie wywoływać więcej niż raz.
		/// </summary>
		private ClimateManager()
		{
			XmlDocument sourceFile = new XmlDocument();
			try
			{
				sourceFile.Load(_fileName);
			}
			catch (Exception exception)
			{
				throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to open file {0}", _fileName), exception);
			}
			XmlNode rootNode = sourceFile.LastChild;
			XmlNodeList nodeList = rootNode.ChildNodes;
			_climates = new Climate[nodeList.Count];
			for (int whichClimate = 0; whichClimate < _climates.Length; ++whichClimate)
			{
				try
				{
					_climates[whichClimate] = new Climate(nodeList[whichClimate]);
				}
				catch (Exception exception)
				{
					throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to create climate object number {0}.", whichClimate), exception);
				}
			}
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
		private const string _fileName = "twa_climates.xml";
		//
		// Stan wewnętrzny:
		//
		private Climate[] _climates;
	}
}
