namespace TWBuildingAssistant.Model.Climate
{
    using System.Collections.Generic;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    public interface IClimate : IParsable
    {
        IEnumerable<IWeatherEffect> WeatherEffects { get; }

        IProvincialEffect GetEffect();

        void SetWeatherParser(Parser<IWeather> parser);

        bool Validate(out string message);

        void RefreshEffect();
    }
}