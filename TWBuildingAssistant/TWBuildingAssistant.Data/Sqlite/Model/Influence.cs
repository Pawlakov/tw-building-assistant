namespace TWBuildingAssistant.Data.Sqlite.Model
{
    public class Influence
    {
        public int Id { get; set; }

        public int? ReligionId { get; set; }

        public int Value { get; set; }

        public int? RegionalEffectId { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}