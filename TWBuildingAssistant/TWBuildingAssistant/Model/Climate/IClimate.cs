namespace TWBuildingAssistant.Model.Climate
{
    using System.Collections.Generic;

    using TWBuildingAssistant.Model.Effects;
    using TWBuildingAssistant.Model.Weather;

    public interface IClimate
    {
        int Id { get; }

        string Name { get; }

        IEnumerable<IWeatherEffect> WeatherEffects { get; }

        IProvincialEffect Effect { get; }

        IParser<IWeather> WeatherParser { set; }

        bool Validate(out string message);

        void RefreshEffect();
    }
}