namespace TWBuildingAssistant.Model.Weather
{
    using System;
    using System.Collections.Generic;

    public interface IConsideredWeatherTracker
    {
        event EventHandler<ConsideredWeatherChangedArgs> ConsideredWeatherChanged;

        IEnumerable<IWeather> ConsideredWeathers { get; }

        void ChangeConsideredWeather(IEnumerable<int> whichWeathers);
    }
}