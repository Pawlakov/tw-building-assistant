namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class TechnologyLevel : ITechnologyLevel
    {
        public TechnologyLevel()
        {
        }

        public TechnologyLevel(ITechnologyLevel source)
        {
            this.Id = source.Id;
            this.FactionId = source.FactionId;
            this.Order = source.Order;
            this.UniversalProvincialEffectId = source.UniversalProvincialEffectId;
            this.AntilegacyProvincialEffectId = source.AntilegacyProvincialEffectId;
        }

        public int Id { get; set; }

        public int FactionId { get; set; }

        public int Order { get; set; }

        public int? UniversalProvincialEffectId { get; set; }

        public int? AntilegacyProvincialEffectId { get; set; }
    }
}