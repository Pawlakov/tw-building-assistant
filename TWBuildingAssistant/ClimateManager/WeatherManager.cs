using System;
using System.Xml;
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
				{
					try
					{
						HiddenSingleton = new WeatherManager();
					}
					catch (Exception exception)
					{
						throw new Exception("Failed to create instance of weather manager.", exception);
					}
				}
				return HiddenSingleton;
			}
		}
		/// <summary>
		/// Tworzy instancję zbioru religii dostępnej później poprzez właściwość Singleton. Nie wywoływać więcej niż raz.
		/// </summary>
		private WeatherManager()
		{
			XmlDocument sourceFile = new XmlDocument();
			try
			{
				sourceFile.Load(_fileName);
			}
			catch (Exception exception)
			{
				throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to open {0}.", _fileName), exception);
			}
			XmlNode rootNode = sourceFile.LastChild;
			XmlNodeList nodeList = rootNode.ChildNodes;
			_weathers = new Weather[nodeList.Count];
			for (int whichWeather = 0; whichWeather < WeatherTypesCount; ++whichWeather)
			{
				try
				{
					_weathers[whichWeather] = new Weather(nodeList[whichWeather]);
				}
				catch (Exception exception)
				{
					throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to create weather object number {0}.", whichWeather), exception);
				}
			}
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
		private const string _fileName = "twa_weathers.xml";
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
			Console.WriteLine("Which weather should be assumed as 'worst case'?.");
			for (int whichWeather = 0; whichWeather < WeatherTypesCount; ++whichWeather)
				Console.WriteLine("{0}. {1}", whichWeather, _weathers[whichWeather].ToString());
			int choice;
			do
			{
				Console.Write("From 0 to {0}: ", WeatherTypesCount - 1);
				try
				{
					choice = Convert.ToInt32(Console.ReadLine(), CultureInfo.InvariantCulture);
				}
				catch (Exception)
				{
					choice = -1;
				}
			} while (choice < 0 || choice > (WeatherTypesCount - 1));
			WorstCaseWeatherIndex = choice;
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
