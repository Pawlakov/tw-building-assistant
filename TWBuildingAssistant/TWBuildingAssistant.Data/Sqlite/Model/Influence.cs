namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Influence : IInfluence
    {
        public Influence()
        {
        }

        public Influence(IInfluence source)
        {
            this.ReligionId = source.ReligionId;
            this.Value = source.Value;
            this.RegionalEffectId = source.RegionalEffectId;
            this.ProvincialEffectId = source.ProvincialEffectId;
        }

        public int Id { get; set; }

        public int? ReligionId { get; set; }

        public int Value { get; set; }

        public int? RegionalEffectId { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}