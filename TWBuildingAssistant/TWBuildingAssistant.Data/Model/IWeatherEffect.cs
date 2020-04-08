namespace TWBuildingAssistant.Data.Model
{
    public interface IWeatherEffect
    {
        int ClimateId { get; }

        int WeatherId { get; }

        int ProvincialEffectId { get; }
    }
}