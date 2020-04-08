namespace TWBuildingAssistant.Data.Model
{
    public interface IInfluence
    {
        int? ReligionId { get; set; }

        int Value { get; }

        int? RegionalEffectId { get; }

        int? ProvincialEffectId { get; }
    }
}