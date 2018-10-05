namespace TWBuildingAssistant.Model.Resources
{
    public interface IResource
    {
        int Id { get; }

        string Name { get; }

        SlotType BuildingType { get; }

        bool Obligatory { get; }

        bool Validate(out string message);
    }
}