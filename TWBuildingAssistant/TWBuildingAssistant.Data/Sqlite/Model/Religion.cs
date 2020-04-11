namespace TWBuildingAssistant.Data.Sqlite.Model
{
    public class Religion
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}