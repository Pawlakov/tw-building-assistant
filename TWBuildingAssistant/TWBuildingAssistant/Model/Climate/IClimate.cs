namespace TWBuildingAssistant.Model.Climate
{
    using System.Collections.Generic;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    public interface IClimate : IParsable
    {
        IEnumerable<IWeatherEffect> WeatherEffects { get; }

        IProvincialEffect Effect { get; }

        Parser<IWeather> WeatherParser { set; }

        bool Validate(out string message);

        void RefreshEffect();
    }
}