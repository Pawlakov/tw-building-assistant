namespace TWBuildingAssistant.Data.Model
{
    public interface IBuildingLevel
    {
        int Id { get; }

        string Name { get; }

        int? ParentBuildingLevelId { get; }

        int? RegionalEffectId { get; }
    }
}