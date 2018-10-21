namespace TWBuildingAssistant.Model.Map
{
    using TWBuildingAssistant.Model.Resources;

    public interface IRegion
    {
        string Name { get; }

        bool IsCity { get; }

        int? ResourceId { get; }

        bool IsCoastal { get; }

        int SlotsCountOffset { get; }

        int GetSlotsCount();
        
        IResource GetResource();

        void SetResourceParser(Parser<IResource> parser);

        bool Validate(out string message);
    }
}