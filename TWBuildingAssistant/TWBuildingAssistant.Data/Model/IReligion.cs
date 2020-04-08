namespace TWBuildingAssistant.Data.Model
{
    public interface IReligion
    {
        int Id { get; }

        string Name { get; }

        int? ProvincialEffectId { get; }
    }
}