namespace TWBuildingAssistant.Data.Model
{
    public interface IBuildingLevelLock
    {
        int BuildingLevelId { get; }

        int TechnologyLevelId { get; }

        bool Antilegacy { get; }

        bool Lock { get; }
    }
}