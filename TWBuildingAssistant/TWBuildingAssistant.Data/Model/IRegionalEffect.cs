namespace TWBuildingAssistant.Data.Model
{
    public interface IRegionalEffect : IProvincialEffect
    {
        int RegionalSanitation { get; }
    }
}