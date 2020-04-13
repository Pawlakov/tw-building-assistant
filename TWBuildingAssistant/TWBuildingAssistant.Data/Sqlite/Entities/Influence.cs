namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    public class Influence
    {
        public int Id { get; set; }

        public int? ReligionId { get; set; }

        public int Value { get; set; }

        public int? EffectId { get; set; }
    }
}