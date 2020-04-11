namespace TWBuildingAssistant.Data.Sqlite.Model
{
    using TWBuildingAssistant.Data.Model;

    internal class Bonus : IBonus
    {
        public Bonus()
        {
        }

        public Bonus(IBonus source)
        {
            this.Value = source.Value;
            this.Category = source.Category;
            this.Type = source.Type;
            this.RegionalEffectId = source.RegionalEffectId;
            this.ProvincialEffectId = source.ProvincialEffectId;
        }

        public int Id { get; set; }

        public int Value { get; set; }

        public IncomeCategory Category { get; set; }

        public BonusType Type { get; set; }

        public int? RegionalEffectId { get; set; }

        public int? ProvincialEffectId { get; set; }
    }
}