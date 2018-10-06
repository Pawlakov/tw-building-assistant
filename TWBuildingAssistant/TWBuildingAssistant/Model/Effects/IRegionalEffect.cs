namespace TWBuildingAssistant.Model.Effects
{
    public interface IRegionalEffect : IProvincialEffect
    {
        int RegionalSanitation { get; }

        IRegionalEffect Aggregate(IRegionalEffect other);
    }
}