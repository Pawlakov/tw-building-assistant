namespace TWBuildingAssistant.Model.Resources
{
    public interface IResource : IParsable
    {
        SlotType BuildingType { get; }

        bool Obligatory { get; }

        bool Validate(out string message);
    }
}