namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class WeatherEffectRepository : IRepository<IWeatherEffect>
    {
        private const string JsonFileName = @"Json\twa_data_weather_effects.json";

        private readonly JsonSource<IWeatherEffect, WeatherEffect> jsonSource;

        public WeatherEffectRepository()
        {
            this.jsonSource = new JsonSource<IWeatherEffect, WeatherEffect>(JsonFileName);
        }

        public IList<IWeatherEffect> DataSet => this.jsonSource.DataSet;
    }
}