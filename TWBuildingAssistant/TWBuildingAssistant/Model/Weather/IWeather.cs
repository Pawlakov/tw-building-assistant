namespace TWBuildingAssistant.Model.Weather
{
    public interface IWeather : IParsable
    {
        IConsideredWeatherTracker ConsideredWeatherTracker { set; }

        bool IsConsidered();

        bool Validate(out string message);
    }
}