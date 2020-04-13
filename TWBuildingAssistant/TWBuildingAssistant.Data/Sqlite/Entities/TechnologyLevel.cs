namespace TWBuildingAssistant.Data.Sqlite.Entities
{
    public class TechnologyLevel
    {
        public int Id { get; set; }

        public int FactionId { get; set; }

        public int Order { get; set; }

        public int? UniversalProvincialEffectId { get; set; }

        public int? AntilegacyProvincialEffectId { get; set; }
    }
}