namespace TWBuildingAssistant.Model.Buildings
{
    using TWBuildingAssistant.Model.Effects;

    public interface IBuilding : IParsable
    {
        IRegionalEffect Effect { get; }

        bool Validate(out string message);
    }
}