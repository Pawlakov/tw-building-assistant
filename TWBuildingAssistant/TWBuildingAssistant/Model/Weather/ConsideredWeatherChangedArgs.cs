namespace TWBuildingAssistant.Model.Weather
{
    using System;
    using System.Collections.Generic;

    public class ConsideredWeatherChangedArgs : EventArgs
    {
        public ConsideredWeatherChangedArgs(IConsideredWeatherTracker tracker, IEnumerable<IWeather> newConsideredWeathers)
        {
            this.Tracker = tracker;
            this.NewConsideredWeathers = newConsideredWeathers;
        }

        public IConsideredWeatherTracker Tracker { get; }

        public IEnumerable<IWeather> NewConsideredWeathers { get; }
    }
}