using System;
using System.Xml;
using System.Globalization;
namespace ClimateAndWeather
{
	public class ClimateManager
	{
		/// <summary>
		/// Odwołanie do tego jedynego obiektu zarządzającego wszystkimi klimatami.
		/// </summary>
		public static ClimateManager Singleton{get; private set;} = null;
		/// <summary>
		/// Tworzy instancję zbioru religii dostępnej później poprzez właściwość Singleton. Nie wywoływać więcej niż raz.
		/// </summary>
		public static ClimateManager CreateSingleton()
		{
			if (Singleton == null)
			{
				Singleton = new ClimateManager();
				return Singleton;
			}
			throw new InvalidOperationException("You cannot create a new singleton when one already exists.");
		}
		//
		//
		//
		private const string _climatesFileName = "twa_climates.xml";
		private const string _weathersFileName = "twa_weathers.xml";
		private Climate[] _climates;
		private Weather[] _weathers;
		private int _worstCaseWeather;
		internal int WeatherTypesCount
		{
			get { return _weathers.Length; }
		}
		private ClimateManager()
		{
			XmlDocument weathersSourceFile = new XmlDocument();
			XmlDocument climatesSourceFile = new XmlDocument();
			try
			{
				weathersSourceFile.Load(_climatesFileName);
			}
			catch (Exception exception)
			{
				throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to open file {0}", _weathersFileName), exception);
			}
			CreateWeathers(weathersSourceFile);
			try
			{
				climatesSourceFile.Load(_climatesFileName);
			}
			catch (Exception exception)
			{
				throw new Exception(String.Format(CultureInfo.CurrentCulture, "Failed to open file {0}", _climatesFileName), exception);
			}
			CreateClimates(climatesSourceFile);
		}
		internal Weather WorstCaseWeather
		{
			get { return _weathers[_worstCaseWeather]; }
		}
		internal string GetWeatherNameByIndex(int index)
		{
			return _weathers[index].Name;
		}
		internal int GetWeatherIndex(Weather weather)
		{
			if (weather == null)
				throw new ArgumentNullException("weather");
			for (int whichWeather = 0; whichWeather < WeatherTypesCount; ++whichWeather)
				if (weather == _weathers[whichWeather])
					return whichWeather;
			throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Unknown Weather object (name: {0})", weather.Name), "weather");
		}
		private void CreateClimates(XmlDocument sourceFile)
		{
			XmlNodeList nodeList = sourceFile.GetElementsByTagName("climate");
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
		private void CreateWeathers(XmlDocument sourceFile)
		{
			XmlNodeList nodeList = sourceFile.GetElementsByTagName("weather");
			_weathers = new Weather[nodeList.Count];
			for (int whichWeather = 0; whichWeather < _climates.Length; ++whichWeather)
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
			PickWorstCaseWeather();
		}
		private void PickWorstCaseWeather()
		{
			int choice = -1;
			Console.WriteLine("Which weather should be assumed as 'worst case'?.");
			for (int whichWeather = 0; whichWeather < WeatherTypesCount; ++whichWeather)
				Console.WriteLine("{0}. {1}", whichWeather, _weathers[whichWeather].Name);
			while (choice < 0 && choice > (WeatherTypesCount - 1))
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
			}
			_worstCaseWeather = choice;
		}
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
	}
}
