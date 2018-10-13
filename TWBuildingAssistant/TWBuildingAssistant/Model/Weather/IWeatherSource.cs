namespace TWBuildingAssistant.Model.Weather
{
    using System.Collections.Generic;

    public interface IWeatherSource
    {
        IEnumerable<IWeather> Weathers { get; }
    }
}