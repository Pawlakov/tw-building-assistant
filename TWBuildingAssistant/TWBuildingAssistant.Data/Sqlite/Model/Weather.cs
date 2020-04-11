namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Weather : IWeather
    {
        public Weather()
        {
        }

        public Weather(IWeather source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}