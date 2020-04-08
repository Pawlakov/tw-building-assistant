namespace TWBuildingAssistant.Data.Json
{
    using System.Collections.Generic;
    using TWBuildingAssistant.Data.JsonModel;
    using TWBuildingAssistant.Data.Model;

    public class WeatherRepository : IRepository<IWeather>
    {
        private const string JsonFileName = @"Json\twa_data_weathers.json";

        private readonly JsonSource<IWeather, Weather> jsonSource;

        public WeatherRepository()
        {
            this.jsonSource = new JsonSource<IWeather, Weather>(JsonFileName);
        }

        public IList<IWeather> DataSet => this.jsonSource.DataSet;
    }
}