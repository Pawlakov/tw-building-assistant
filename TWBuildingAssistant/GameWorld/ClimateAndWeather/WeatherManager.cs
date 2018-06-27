using System;
using EnumsNET;
using System.Globalization;
namespace GameWorld.ClimateAndWeather
{
	// Do zarządzania pogodami.
	public class WeatherManager
	{
		public WeatherManager()
		{
			WeatherTypesCount = Enums.GetMemberCount<Weather>();
			WorstCaseWeather = Weather.Normal;
		}
		//
		// W momencie po zmianie najgorszej przewidywanej pogody.
		public event WorstCaseWeatherChangedHandler WorstCaseWeatherChanged;
		//
		public int WeatherTypesCount { get; }
		public Weather WorstCaseWeather { get; private set; }
		//
		// Zmiana najgorszej przewidywanej pogody.
		public void ChangeWorstCaseWeather(Weather whichWeather)
		{
			WorstCaseWeather = whichWeather;
			OnWorstCaseWeatherChanged(EventArgs.Empty);
		}
		private void OnWorstCaseWeatherChanged(EventArgs e)
		{
			WorstCaseWeatherChanged?.Invoke(this, e);
		}
	}
	public delegate void WorstCaseWeatherChangedHandler(WeatherManager sender, EventArgs e);
}
