namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class WeatherEffect : IWeatherEffect
    {
        public WeatherEffect()
        {
        }

        public WeatherEffect(IWeatherEffect source)
        {
            this.ClimateId = source.ClimateId;
            this.WeatherId = source.WeatherId;
            this.ProvincialEffectId = source.ProvincialEffectId;
        }

        public int ClimateId { get; set; }

        public int WeatherId { get; set; }

        public int ProvincialEffectId { get; set; }
    }
}