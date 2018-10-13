namespace TWBuildingAssistant.Model.Weather
{
    public interface IWeather
    {
        int Id { get; }

        string Name { get; }

        IConsideredWeatherTracker ConsideredWeatherTracker { set; }

        bool IsConsidered();

        bool Validate(out string message);
    }
}