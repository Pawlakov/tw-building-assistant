namespace TWBuildingAssistant.Model.Effects
{
    public interface IRegionalEffect : IProvincionalEffect
    {
        int? RegionalSanitation { get; }

        IRegionalEffect Aggregate(IRegionalEffect other);
    }
}