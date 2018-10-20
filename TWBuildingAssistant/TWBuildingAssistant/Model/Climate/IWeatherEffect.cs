namespace TWBuildingAssistant.Model.Climate
{
    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    public interface IWeatherEffect
    {
        int WeatherId { get; }

        IProvincialEffect Effect { get; }

        IWeather GetWeather();

        void SetWeatherParser(Parser<IWeather> parser);

        bool Validate(out string message);
    }
}