namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    public class Religion
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}