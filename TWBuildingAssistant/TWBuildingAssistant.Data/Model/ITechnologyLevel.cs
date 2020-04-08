namespace TWBuildingAssistant.Data.Model
{
    public interface ITechnologyLevel
    {
        int Id { get; }

        int FactionId { get; }

        int Order { get; }

        int? UniversalProvincialEffectId { get; }

        int? AntilegacyProvincialEffectId { get; }
    }
}