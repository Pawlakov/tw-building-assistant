namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Climate : IClimate
    {
        public Climate()
        {
        }

        public Climate(IClimate source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}