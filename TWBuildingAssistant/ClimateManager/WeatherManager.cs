using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Globalization;
namespace ClimateAndWeather
{
	/// <summary>
	/// Instancja tej klasy zarządza wszystkimi dostępnymi typami pogody.
	/// </summary>
	public class WeatherManager
	{
		// Singleton:
		//
		private static WeatherManager HiddenSingleton { get; set; } = null;
		/// <summary>
		/// Odwołanie do tego jedynego obiektu zarządzającego wszystkimi klimatami.
		/// </summary>
		public static WeatherManager Singleton
		{
			get
			{
				if (HiddenSingleton == null)
					HiddenSingleton = new WeatherManager();
				return HiddenSingleton;
			}
		}
		/// <summary>
		/// Tworzy instancję zbioru religii dostępnej później poprzez właściwość Singleton. Nie wywoływać więcej niż raz.
		/// </summary>
		private WeatherManager()
		{
			XDocument sourceFile;
			sourceFile = XDocument.Load(_sourceFilename);
			_weathers = (from element in sourceFile.Root.Elements() select new Weather(element)).ToArray();
		}
		//
		// Interfejs wewnętrzny:
		//
		internal event WorstCaseWeatherChangedHandler WorstCaseWeatherChanged;
		internal event WorstCaseWeatherChangingHandler WorstCaseWeatherChanging;
		//
		internal int WeatherTypesCount
		{
			get { return _weathers.Length; }
		}
		internal int WorstCaseWeatherIndex { get; private set; } = -1;
		//
		// Stałe:
		//
		private const string _sourceFilename = "twa_weathers.xml";
		//
		// Stan wewnętrzny:
		//
		private readonly Weather[] _weathers;
		//
		// Interfejs publiczny:
		//
		/// <summary>
		/// Pyta użytkownika o dokonanie wyboru która pogoda ma być przyjęta za najgorszy przypadek.
		/// </summary>
		public void ChangeWorstCaseWeather()
		{
			OnWorstCaseWeatherChanging(EventArgs.Empty);
			Console.WriteLine("Which weather should be assumed as worst case:");
			for (int whichWeather = 0; whichWeather < WeatherTypesCount; ++whichWeather)
				Console.WriteLine("{0}. {1}", whichWeather, _weathers[whichWeather].ToString());
			do
			{
				Console.Write("From 0 to {0}: ", WeatherTypesCount - 1);
				try
				{
					WorstCaseWeatherIndex = Convert.ToInt32(Console.ReadLine(), CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					WorstCaseWeatherIndex = -1;
				}
			} while (WorstCaseWeatherIndex < 0 || WorstCaseWeatherIndex > (WeatherTypesCount - 1));
			OnWorstCaseWeatherChanged(EventArgs.Empty);
		}
		//
		// Pomocnicze:
		//
		private void OnWorstCaseWeatherChanging(EventArgs e)
		{
			WorstCaseWeatherChanging?.Invoke(this, e);
		}
		private void OnWorstCaseWeatherChanged(EventArgs e)
		{
			WorstCaseWeatherChanged?.Invoke(this, e);
		}
	}
	internal delegate void WorstCaseWeatherChangedHandler(WeatherManager sender, EventArgs e);
	internal delegate void WorstCaseWeatherChangingHandler(WeatherManager sender, EventArgs e);
}