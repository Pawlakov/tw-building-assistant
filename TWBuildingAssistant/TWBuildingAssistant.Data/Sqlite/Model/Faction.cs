namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Faction : IFaction
    {
        public Faction()
        {
        }

        public Faction(IFaction source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}