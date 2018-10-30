namespace TWBuildingAssistant.Model.Weather
{
    public interface IWeather : IParsable
    {
        IConsideredWeatherTracker ConsideredWeatherTracker { set; }

        bool IsConsideredByDefault { get; }

        bool IsConsidered();

        bool Validate(out string message);
    }
}