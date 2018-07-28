namespace TWBuildingAssistant.Model.ClimateAndWeather
{
    using System;

    using EnumsNET;

    public class WeatherManager
    {
        public WeatherManager()
        {
            WeatherTypesCount = Enums.GetMemberCount<Weather>();
            WorstCaseWeather = Weather.Normal;
        }

        public event WorstCaseWeatherChangedHandler WorstCaseWeatherChanged;

        public int WeatherTypesCount { get; }

        public Weather WorstCaseWeather { get; private set; }

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
