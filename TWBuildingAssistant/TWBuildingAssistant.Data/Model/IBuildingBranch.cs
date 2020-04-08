namespace TWBuildingAssistant.Data.Model
{
    public interface IBuildingBranch
    {
        int Id { get; }

        string Name { get; }

        SlotType SlotType { get; }

        RegionType? RegionType { get; }

        bool AllowParallel { get; }

        int RootBuildingLevelId { get; }

        int? ReligionId { get; }

        int? ResourceId { get; }
    }
}