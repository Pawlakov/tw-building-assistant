namespace TWBuildingAssistant.Data.Model
{
    public interface IRegion
    {
        int Id { get; }

        string Name { get; }

        bool IsCity { get; }

        int? ResourceId { get; }

        bool IsCoastal { get; }

        int SlotsCountOffset { get; }

        int ProvinceId { get; }
    }
}