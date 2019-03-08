namespace TWBuildingAssistant.Model.Buildings
{
    using System.Collections.Generic;

    public interface IBranch : IParsable
    {
        SlotType SlotType { get; }

        RegionType RegionType { get; }

        IList<IBuilding> Levels { get; }

        bool Validate(out string message);
    }
}