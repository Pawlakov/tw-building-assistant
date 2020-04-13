namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    public class TechnologyLevel
    {
        public int Id { get; set; }

        public int FactionId { get; set; }

        public int Order { get; set; }

        public int? UniversalEffectId { get; set; }

        public int? AntilegacyEffectId { get; set; }
    }
}