namespace TWBuildingAssistant.Data.Model
{
    public interface IProvince
    {
        int Id { get; }

        string Name { get; }

        int Fertility { get; }

        int ClimateId { get; }

        int? ProvincialEffectId { get; }
    }
}