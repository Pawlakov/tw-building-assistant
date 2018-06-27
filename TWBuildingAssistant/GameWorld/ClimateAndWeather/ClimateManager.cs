using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
namespace GameWorld.ClimateAndWeather
{
	// Klasa do zarządzania klimatami.
	public class ClimateManager : Map.IClimateParser
	{
		public ClimateManager()
		{
			WeatherManager = new WeatherManager();
			XDocument sourceFile = XDocument.Load(_sourceFilename);
			_climates = (from XElement element in sourceFile.Root.Elements() select new Climate(element, WeatherManager)).ToArray();
		}
		//
		// Zidentyfikuj odpowiedni klimat na podstawie nazwy.
		Climate Map.IClimateParser.Parse(string input)
		{
			if (input == null)
				return null;
			foreach (Climate climate in _climates)
			{
				if(input.Equals(climate.Name, StringComparison.OrdinalIgnoreCase))
					return climate;
			}
			return null;
		}
		public void ChangeWorstCaseWeather(Weather whichWeather)
		{
			WeatherManager.ChangeWorstCaseWeather(whichWeather);
		}
		//
		private const string _sourceFilename = "ClimateAndWeather\\twa_climates.xml";
		//
		// Możliwe klimaty.
		private readonly Climate[] _climates;
		//
		public WeatherManager WeatherManager { get; }
	}
}
