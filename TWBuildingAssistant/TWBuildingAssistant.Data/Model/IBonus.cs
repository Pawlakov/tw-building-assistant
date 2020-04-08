namespace TWBuildingAssistant.Data.Model
{
    public interface IBonus
    {
        int Value { get; }

        IncomeCategory Category { get; }

        BonusType Type { get; }

        int? RegionalEffectId { get; }

        int? ProvincialEffectId { get; }
    }
}