namespace TWBuildingAssistant.Model.Climate
{
    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    public interface IWeatherEffect
    {
        int WeatherId { get; }

        IWeather Weather { get; }

        IProvincialEffect Effect { get; }

        IParser<IWeather> WeatherParser { set; }

        bool Validate(out string message);
    }
}