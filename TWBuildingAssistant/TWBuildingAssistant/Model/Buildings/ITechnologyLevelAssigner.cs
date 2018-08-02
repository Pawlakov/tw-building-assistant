namespace TWBuildingAssistant.Model.Buildings
{
    public interface ITechnologyLevelAssigner
    {
        Technologies.TechnologyLevel GetLevel(int level, bool? useLegacy);
    }
}
